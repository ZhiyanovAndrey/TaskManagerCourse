namespace TaskManagerCourse.Api.Models
{
    public class Desk:CommonObject
    {
        public int Id { get; set; }

        public bool isPrivate { get; set; }

        public string Columns { get; set; } // хранение в json формате

        //  Доска принадлежит Admin, он  может быть только один
        public int AdminId { get; set; }
        public User Admin { get; set; }


        // Доска принадлежит проекту, покажем БД что это ключ на Project ниже
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        // задача принадлежит доске
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
