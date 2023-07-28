using Microsoft.AspNetCore.Mvc;
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

        public ProjectModel Get(int id)
        {
            Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
            return project?.ToDto();
        }

        public bool Update(int id, ProjectModel model)
        {
            bool result = DoAction(delegate ()
            {
                Project newproject = _db.Projects.FirstOrDefault(p => p.Id == id);
                newproject.Name = model.Name;
                newproject.Description = model.Description; 
                newproject.Status = model.Status;
                newproject.Photo=model.Photo;   
                newproject.AdminId= model.AdminId;
                _db.Projects.Update(newproject);
                _db.SaveChanges();

            });
            return result;
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
