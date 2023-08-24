using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;

namespace TaskManagerCourse.Api.Controllers
{
    // api указывается сразу после http://localhost:5171/api
    [Authorize(Roles = "Admin")] // требует авторизации для всех членов класса
    [Route("api/[controller]")] // атрибут указывает как мы будем обращаться с Фронта api/users название первой части от контроллера UsersController
    [ApiController]
    public class UsersController : ControllerBase
    {
        // API для получения создания и удаления пользователя
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;

        public UsersController(ApplicationContext db)
        {
            _db = db;
            _usersService= new UsersService(db);

        }
        // тестовый запрос строка
        [AllowAnonymous] // не требует авторизации
        [HttpGet("test")]
        public IActionResult Test()
        {
            string t = $"";
            return Ok($"Привет! Сервер запущен {DateTime.Now.ToString("D")} в {DateTime.Now.ToString("t")}");
        }

        // запрос на создание User 
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel userModel) // UserModel получаем из тела запроса
        {
            if (userModel != null)
            {
                // получим User из фронта UserModel
                bool result = _usersService.Create(userModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        // получаем id User из URL
        [HttpPatch("{id}")]
        // запрос на изменение User 
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
               // обращение к БД вынесли в UserService, проверку на null можно вынести или оставить
                bool result = _usersService.Update(id, userModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        // получаем id User из URL
        [HttpDelete("{id}")]
        // запрос на удаление User 
        public IActionResult DeleteUser(int id) // может не принимать модель, а только id
        {
            // обращение к БД вынесли в UserService, а сдесь обращаемся к _userService
            bool result = _usersService.Delete(id);
            return result ? Ok() : NotFound();
            
        }

        // возвращаем весь список из UserModel
        // не указываем ни чего в скобках и [HttpGet] тоже можно не писать
        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers() // меняем IEnumerable на Task для использования ToListAsync() вместо ToList()
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();    
        }

        // запрос на массовое добавление
        [HttpPost("all")]
        public async Task<IActionResult> CreateMultipleUsers([FromBody] List<UserModel> userModels) // 
        {
            // проверку на null не обязательно выносить в UserService можно оставить
            // верхнеуровневые проверки лучше выносить на верх
            if (userModels != null && userModels.Count>0)
            {
                bool result = _usersService.CreateMultipleUsers(userModels);
                return result ? Ok() : NotFound();
            }
            return BadRequest(userModels);
        }
    }
}   
