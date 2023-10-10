using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Models;
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

        // убираем [Authorize(Roles = "Admin")]  и делаем проверку с пом  if (user.Status == UserStatus.Admin....
        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> Get()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user.Status == UserStatus.Admin)
            {

                return await _projectsService.GetAll().ToListAsync();
            }
            else
            {
                return await _projectsService.GetByUserId(user.Id);
            }
        }

        // возврат объекта по Id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            var project = _projectsService.Get(id);
            return project == null ? NoContent() : Ok(project);

        }




        [HttpPost]
        public IActionResult Create(int id, [FromBody] ProjectModel projectModel) // ProjectModel получаем из тела запроса
        {
            if (projectModel != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);

                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        var admin = _db.ProjectAdmins.FirstOrDefault(p => p.UserId == user.Id);
                        if (admin == null) // если нет админа создадим его
                        {
                            admin = new ProjectAdmin(user);
                            _db.ProjectAdmins.Add(admin);
                            _db.SaveChanges();
                        }
                        projectModel.AdminId = admin.Id;

                        // получим User из фронта UserModel
                        bool result = _projectsService.Create(projectModel);
                        return result ? Ok() : NotFound();
                    }
                }
                return Unauthorized();
            }
            return BadRequest();

        }



        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] ProjectModel projectModel)
        {
            if (projectModel != null)
            {
                // получим User из фронта UserModel
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        bool result = _projectsService.Update(id, projectModel);
                        return result ? Ok() : NotFound();
                    }


                    return Unauthorized();
                }
            }
            return BadRequest();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _projectsService.Delete(id);
            return result ? Ok() : NotFound();

        }

        // добавление пользователя в проект
        [HttpPatch("{id}/users")]
        public IActionResult AddUsersToProject(int id, [FromBody] List<int> userIds)
        {
            if (userIds != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);

                if (user!=null)
                {

                if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                {
                    _projectsService.AddUserToProject(id, userIds);
                        return Ok();    
          
                }
                return Unauthorized();
                }
            }
            return BadRequest();
        }
    // удаление пользователя из проекта
        [HttpPatch("{id}/users/remove")]
        public IActionResult RemoveUsersToProject(int id, [FromBody] List<int> userIds)
        {
            if (userIds != null)
            {
                var user = _usersService.GetUser(HttpContext.User.Identity.Name);

                if (user != null)
                {

                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _projectsService.RemoveUserToProject(id, userIds);
                        return Ok();

                    }
                    return Unauthorized();
                }
            }
            return BadRequest();
        }
    }
}
