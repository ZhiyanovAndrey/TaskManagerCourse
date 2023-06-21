namespace TaskManagerCourse.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginDate{ get; set; }

        public byte[] Photo{ get; set; } // массив byte

        public UserStatus Status { get; set; }

        // связи многие ко многим

        public List<Project> Projects { get; set; } = new List<Project>(); 

        public List<Desk> Desks { get; set; } = new List<Desk>();

        public List<Task> Tasks { get; set; } = new List<Task>();

        // пустой конструктор 
        public User() { }

        // конструктор, чтобы сразу задать пользователяб статус задан по умолчанию

        public User(string sname, string name, string email, string pass, string phone = null,
            UserStatus status = UserStatus.User, byte[] photo = null)
        {
            Surname= sname; 
            Name = name;    
            Email = email;
            Password = pass;
            Phone = phone;  
            Status = status;
            Photo = photo;
            RegistrationDate= DateTime.Now;

        }


    }
}
