using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public DesksController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
           


        }
        // Получение всех Desks которыми владеет пользователь
        [HttpGet]
        public async Task<IEnumerable<CommonModel>> GetDeskForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            
        }

        // GET api/<DesksController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           
        }
        // получение Desck по id проекта
        [HttpGet("{id}")]
        public IActionResult GetProjectDesks(int id)
        {
           
        }

        [HttpPost]
        public IActionResult Create([FromBody] string value)
        {
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
        }
    }
}
