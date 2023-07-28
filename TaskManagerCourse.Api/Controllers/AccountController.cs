using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerCourse.Api.Models.Data;
using System.Linq;
using TaskManagerCourse.Api.Models.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TaskManagerCourse.Api.Models;
using TaskManager.Common.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")] // атрибут указывает как мы будем обращаться с Фронта api/users название первой части от контроллера Account
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _userService;

        public AccountController(ApplicationContext db)
        {
            _db = db;
            _userService = new UsersService(db);
        }

        [Authorize] // запрет на получение инфо если не авторизировался
        [HttpGet("info")]

        public IActionResult GetCarentUserInfo()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Email == userName);
            if (user != null)
            {
                return Ok(user.ToDto()); // 
            }

            return NotFound();
        }

        // тестовый запрос строка
        [HttpGet("test")]
        public IActionResult Test()
        {
            string t = $"";
            return Ok($"Привет! Сервер запущен {DateTime.Now.ToString("D")} в {DateTime.Now.ToString("t")}");
        }

        [HttpPost("token")]

        public IActionResult GetToken()
        {
            var userData = _userService.GetUserLoginPassFromBasicAuth(Request); // передаем Request и  получаем пользователя из 
            var login = userData.Item1;
            var pass = userData.Item2;
            var identity = _userService.GetIdentity(login, pass);

            if (identity == null)
            {
                return BadRequest("Неправильный логин или пароль");
            }

            //создаем экземпляр токена
           var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }

        [Authorize] // запрет на обновление если не авторизировался
        [HttpPatch("update")]
        // запрос на изменение User 
        public IActionResult Update([FromBody] UserModel model)
        {
            if (model != null)
            {
                string userName = HttpContext.User.Identity.Name;

                User userForUpdate = _db.Users.FirstOrDefault(u => u.Email == userName);
                if (userForUpdate != null)
                {


                    userForUpdate.Surname = model.Surname;
                    userForUpdate.Name = model.Name;

                    userForUpdate.Password = model.Password;
                    userForUpdate.Phone = model.Phone;

                    userForUpdate.Photo = model.Photo;

                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();
                    return Ok();


                }
                return NotFound();
            }
            return BadRequest();
        }


    }
}

