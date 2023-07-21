using Microsoft.AspNetCore.Mvc;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Models.Services
{
    public class UsersService : ICommonServiсe<UserModel>
    {
        private readonly ApplicationContext _db;

        // конструктор класса
        public UsersService(ApplicationContext db)
        {
            _db = db;

        }

        public bool Create(UserModel model)
        {


            return DoAction(delegate ()
            {
                // получим User из фронта UserModel обновить и перезаписать
                User newUser = new User(model.Surname, model.Name, model.Email,
         model.Password, model.Phone, model.Status, model.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();

            });
        }

        public bool Delete(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return DoAction(delegate ()
                {
                    _db.Users.Remove(user);
                    _db.SaveChanges();

                });

            }
            return false;
        }

        public bool Update(int id, UserModel model)
        {
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userForUpdate != null)
            {
                return DoAction(delegate ()
                {
                    userForUpdate.Surname = model.Surname;
                    userForUpdate.Name = model.Name;
                    userForUpdate.Email = model.Email;
                    userForUpdate.Password = model.Password;
                    userForUpdate.Phone = model.Phone;
                    userForUpdate.Status = model.Status;
                    userForUpdate.Photo = model.Photo;

                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();

                });
            }
            return false;

        }

        public bool CreateMultipleUsers([FromBody] List<UserModel> userModels) // 
        {


            return DoAction(delegate ()
            {
                var newUsers = userModels.Select(u => new User(u)); // для UserModel нет конструктора создадим его в Users
                _db.Users.AddRange(newUsers);
                _db.SaveChanges();

            });



        }

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
