using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly DesksService _desksService;

        public DesksController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db); // создаем поле выше и отправляем в него _db         
            _desksService = new DesksService(db);


        }
        // Получение всех Desks которыми владеет пользователь
        [HttpGet]
        public async Task<IEnumerable<CommonModel>> GetDeskForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name); // получение пользователя
            if (user != null)
            {
                return await _desksService.GetAll(user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>(); // для возврата пустого списка
        }

        // GET api/<DesksController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // получение пользователя не обязательно, если desk есть то возвращаем ее из Get
            var desk = _desksService.Get(id);

            return desk == null ? NotFound() : Ok(desk);
        }
        // получение Desck по id проекта
        [HttpGet("{project/projectId}")]
        public IActionResult GetProjectDesks(int projectId)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name); // получение пользователя
            if (user != null)
            {
                var desks = _desksService.GetProjectDesks(projectId, user.Id).ToListAsync();
                return Ok(desks);
            }
            return Array.Empty<CommonModel>();
        }

        [HttpPost]
        public IActionResult Create([FromBody] DeskModel deskmodel)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name); // получение пользователя
            if (user != null)
            {
                if (deskmodel != null)
                {
                    bool rezult = _desksService.Create(deskmodel);
                    return rezult ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskModel deskmodel)
        {
// тот же Create 
            var user = _usersService.GetUser(HttpContext.User.Identity.Name); 
            if (user != null)
            {
                if (deskmodel != null)
                {
                    bool rezult = _desksService.Update(id, deskmodel);
                    return rezult ? Ok() : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool rezult = _desksService.Delete(id);
            return rezult ? Ok() : NotFound();
        }
    }
}
