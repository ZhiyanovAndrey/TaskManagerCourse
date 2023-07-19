using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerCourse.Api.Models.Data;
using System.Linq;
using TaskManagerCourse.Api.Models.Services;

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")] // атрибут указывает как мы будем обращаться с Фронта api/Account название первой части от контроллера 
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;

        public AccountController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
        }

      


        //[HttpGet("info")]
        //public IActionResult GetCarentUserInfo()
        //{
        //    string userName = HttpContext.User.Identity.Name;
        //    var user = _db.Users.FirstOrDefault(u => u.Email == userName);
        //    if (user != null)
        //    {
        //        return Ok(User.ToDto());
        //    }

        //    return NotFound();
        //}

        // метод для получения токена

        //public IActionResult GetToken()
        //{




        //}    
    }
}

