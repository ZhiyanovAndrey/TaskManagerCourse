namespace TaskManagerCourse.Api.Models
{
    public class Task:CommonObject
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte[] File { get; set; }

// одной доске принадлежит много задач
        public int DeskId { get; set; }
        public Desk Desk { get; set; }

        public string Column { get; set; }


        public int? CreatorId { get; set; } // может и не быть в проекте допускаем null
        public User Creator { get; set; }

        public int? ExecuterId { get; set; }
        //public User Executer { get; set; }
    }
}
