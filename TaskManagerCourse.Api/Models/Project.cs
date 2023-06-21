namespace TaskManagerCourse.Api.Models
{
    public class Project:CommonObject
    {
        public int Id { get; set; }

        public List<User> AllUsers { get; set; } = new List<User>();

        public List<Desk> AllDecks { get; set; } = new List<Desk>(); // приватная доска для пользователя

        public ProgectStatus Status { get; set; }

// один админ у проекта с 
        public int? AdminId { get; set; }
        public ProjectAdmin Admin { get; set; }


    }
}
