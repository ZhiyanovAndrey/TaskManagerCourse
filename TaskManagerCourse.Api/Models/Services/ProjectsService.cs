using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Models.Services
{
    public class ProjectsService : ICommonServiсe<ProjectModel>
    {
        private readonly ApplicationContext _db;


        public ProjectsService(ApplicationContext db)
        {
            _db = db;

        }

        public bool Create(ProjectModel model)
        {
            // добавим ссылку на админа

            bool result = DoAction(delegate ()
            {
                Project newproject = new Project(model);
                _db.Projects.Add(newproject);
                _db.SaveChanges();

            });
            return result;
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Project newproject = _db.Projects.FirstOrDefault(p => p.Id == id);
                _db.Projects.Remove(newproject);
                _db.SaveChanges();

            });
            return result;
        }


        public bool Update(int id, ProjectModel model)
        {
            bool result = DoAction(delegate ()
            {
                Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
                project.Name = model.Name;
                project.Description = model.Description;
                project.Status = model.Status;
                project.Photo = model.Photo;
                project.AdminId = model.AdminId;
                _db.Projects.Update(project);
                _db.SaveChanges();

            });
            return result;
        }
        /// <summary>
        /// возвращает обьект по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectModel Get(int id)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);
            var projectModel = project?.ToDto();
            if (projectModel!=null)
            {
                projectModel.AllUsersIds = project.AllUsers.Select(u => u.Id).ToList(); // вытаскиваем всех Users и с помощью select отбираем только  id пользователя
            }
            return projectModel; // project?.ToDto(); // при возврате всех данных не только ID урок 4.4
        }

        /// <summary>
        /// метод используем только в том случае если user не равен admin
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ProjectModel>> GetByUserId(int userId)
        {
            List<ProjectModel> result = new List<ProjectModel>();
            var admin = _db.Projects.FirstOrDefault(p => p.AdminId == userId);

            if (admin != null)
            {
                // получим все проекты у админа в подчинении
                var projectForAdmin = await _db.Projects.Where(p => p.AdminId == admin.Id).Select(p => p.ToDto()).ToListAsync(); // select преобразует в projectModel
                result.AddRange(projectForAdmin);
            }
            var projectForUser = await _db.Projects.Include(p => p.AllUsers).Where(p => p.AllUsers.Any(u => u.Id == userId)).Select(p => p.ToDto()).ToListAsync();
            result.AddRange(projectForUser);
            return result;
        }

        public IQueryable<ProjectModel> GetAll()
        {
            return _db.Projects.Select(p => p.ToDto());

        }

        public void AddUserToProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.FirstOrDefault(p => p.Id == id);

            foreach (var userid in userIds)
            {

                var user = _db.Users.FirstOrDefault(u => u.Id == userid);
                project.AllUsers.Add(user);

            }
            _db.SaveChanges();

        }

        public void RemoveUserToProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id); // возвращаем из базы данных проект вместе со всеми Users

            foreach (var userid in userIds)
            {

                var user = _db.Users.FirstOrDefault(u => u.Id == userid);
                if (project.AllUsers.Contains(user))
                {
                    project.AllUsers.Remove(user);
                }


            }
            _db.SaveChanges();

        }

        //public bool CreateMultipleUsers([FromBody] List<UserModel> userModels) // 
        //{


        //    return DoAction(delegate ()
        //    {
        //        var newUsers = userModels.Select(u => new User(u)); // для UserModel нет конструктора создадим его в Users
        //        _db.Users.AddRange(newUsers);
        //        _db.SaveChanges();

        //    });



        //}

        /* что бы не повторять try
         * метод выполняет какое то действие внутри try cach, возвращает bool
         * передаем в него делегат Action и выполняем его в теле try
         * 
         */

        private bool DoAction(Action action)
        {
            try
            {
                action.Invoke(); // вызываем методы сообщенные с делегатом
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
