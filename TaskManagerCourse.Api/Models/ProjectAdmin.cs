namespace TaskManagerCourse.Api.Models
{
    public class ProjectAdmin
    {
        // может быть участником и создателем
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; } 

        // много проектов у админа
        public List<Project> Projects { get; set; } = new List<Project>();


        public ProjectAdmin() { } // без пустого конструктора не работает миграция
        
        public ProjectAdmin(User user) // принимает пользователя и присваевает ему значение ID
        {
            UserId = user.Id; 
            User = user;    // необязательно
        }
     
    }
}
