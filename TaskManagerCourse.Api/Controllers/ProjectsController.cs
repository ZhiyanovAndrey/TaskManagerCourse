using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {

        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly ProjectsService _projectsService;

        public ProjectsController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _projectsService = new ProjectsService(db);

        }
        // возврат объекта по Id
        [HttpGet("{id}")]
        public IActionResult Get(int id) 
        {
            var project = _projectsService.Get(id);
            return project == null ? NoContent() : Ok();

        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> Get()
        {
            return await _db.Projects.Select(p => p.ToDto()).ToListAsync();

        }

        [HttpPost]
        public IActionResult Create(int id, [FromBody] ProjectModel projectModel) // ProjectModel получаем из тела запроса
        {
            if (projectModel != null)
            {
                // получим User из фронта UserModel
                bool result = _projectsService.Create(projectModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();

        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                // получим User из фронта UserModel
                bool result = _projectsService.Update(id, projectModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool result = _projectsService.Delete(id);
            return result ? Ok() : NotFound();

        }
    }
}
