using TaskManager.Common.Models;

namespace TaskManagerCourse.Api.Models
{
    public class Project : CommonObject
    {
        public int Id { get; set; }
        // один админ у проекта с 
        public int? AdminId { get; set; }

        public ProjectAdmin Admin { get; set; }

        public List<User> AllUsers { get; set; } = new List<User>(); // многие ко многим с User

        public List<Desk> AllDecks { get; set; } = new List<Desk>(); // приватная доска для пользователя

        public ProgectStatus Status { get; set; }


        public Project() { }

        public Project(ProjectModel projectModel) : base(projectModel) // наследуемся от конструктора базового класса Project и все свойства от CommonObject передаду
        {
            Id = projectModel.Id;
            AdminId = projectModel.AdminId;
            Status = projectModel.Status;
        }


        // применим патерн DTO
        public ProjectModel ToDto()
        {
            return new ProjectModel()
            {
                Id=this.Id, 
                Name=this.Name,
                Description=this.Description,
                Photo = this.Photo,
                CreationDate = this.CreationDate, 
                AdminId=this.AdminId,  
                Status=this.Status
                
            };
        }
    }
}
