using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Models.Services
{
    public class UsersService : AbstractionService, ICommonServiсe<UserModel> 
    {
        private readonly ApplicationContext _db;

        // конструктор класса
        public UsersService(ApplicationContext db)
        {
            _db = db;

        }

        // метод 
        /// <summary>
        /// метод получает Логин и Пароль возвращает кортэж (двойной компонент)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Tuple<string, string> GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            string userLogin = "";
            string userPass = "";
            string authHeader = request.Headers["Authorization"].ToString(); // dictionary поэтому делаем проверку по ключу
            if (authHeader != null) //  && authHeader.StartsWith("Basic)" базовая передача
            {
                string encodedUserNamePass = authHeader.Replace("Basic ", ""); // оставляем данные только логин и пароль без Basic
                var encoding = Encoding.GetEncoding("iso-8859-1");
                string[] namePassArr = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(":");
                userLogin = namePassArr[0];
                userPass = namePassArr[1];


            }
            return new Tuple<string, string>(userLogin, userPass);
        }


        public User GetUser(string login, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
            return user;

        }

        public User GetUser(string login)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login);
            return user;

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

        public UserModel Get(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            return user?.ToDto(); // проверка на null, вернет null не будет пытаться вызвать ToDto().
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User currentUser = GetUser(username, password);
            if (currentUser != null)
            {
                currentUser.LastLoginDate = DateTime.Now;
                _db.Users.Update(currentUser);
                _db.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email), // указываем, что Майл определяющее свойство
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.Status.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public IEnumerable<UserModel> GetAllByIds(List<int> userIds)
        {
            foreach (var id in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == id).ToDto();
                yield return user;
            }

        }
    }
}
