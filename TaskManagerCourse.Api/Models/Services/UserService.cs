using Microsoft.AspNetCore.Mvc;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Models.Services
{
    public class UserService : ICommonServiсe<UserModel>
    {
        private readonly ApplicationContext _db;

        public UserService(ApplicationContext db)
        {
            _db = db;
        }

        public bool Create(UserModel model)
        {
            try
            {
                // получим User из фронта UserModel обновить и перезаписать
                User newUser = new User(model.Surname, model.Name, model.Email,
         model.Password, model.Phone, model.Status, model.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                try
                {
                    _db.Users.Remove(user);
                    _db.SaveChanges();
                    return true;

                }

                catch (Exception)
                {

                    return false;
                }
            }
            return false;
        }

        public bool Update(int id, UserModel model)
        {
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userForUpdate != null)
            {
                try
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
                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        public bool CreateMultipleUsers([FromBody] List<UserModel> userModels) // 
        {

            try
            {
                var newUsers = userModels.Select(u => new User(u)); // для UserModel нет конструктора создадим его в Users
                _db.Users.AddRange(newUsers);
                _db.SaveChanges();
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }

    }
}
