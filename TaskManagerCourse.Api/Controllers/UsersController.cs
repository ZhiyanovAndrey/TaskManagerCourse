using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Controllers
{
    // api указывается сразу после http://localhost:5171/
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // API для получения создания и удаления пользователя
        private readonly ApplicationContext _db;

        public UsersController(ApplicationContext db)
        {
            _db = db;
        }
        // тестовый запрос строка
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Привет!");
        }

        // запрос на создание User 
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                // получим User из фронта UserModel
                User newUser = new User(userModel.Surname, userModel.Name, userModel.Email,
                    userModel.Password, userModel.Phone, userModel.Status, userModel.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        // получаем id User из URL
        [HttpPatch("update/{id}")]
        // запрос на изменение User 
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                // получим User из фронта UserModel обновить и перезаписать
                User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
                if (userForUpdate != null)
                {
                    userForUpdate.Surname = userModel.Surname;
                    userForUpdate.Name = userModel.Name;
                    userForUpdate.Email = userModel.Email;
                    userForUpdate.Password = userModel.Password;
                    userForUpdate.Phone = userModel.Phone;
                    userForUpdate.Status = userModel.Status;
                    userForUpdate.Photo = userModel.Photo;

                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();
                    return Ok();
                }

                return NotFound();
            }
            return BadRequest();
        }

        // получаем id User из URL
        [HttpPatch("delete/{id}")]
        // запрос на удаление User 
        public IActionResult DeleteUser(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        // возвращаем весь список из UserModel
        public async Task<IEnumerable<UserModel>> GetUsers() // меняем IEnumerable на Task для использования ToListAsync() вместо ToList()
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();    
        }
    }
}
