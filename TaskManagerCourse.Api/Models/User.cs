using TaskManager.Common.Models;

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

        public DateTime LastLoginDate { get; set; }

        public byte[] Photo { get; set; } // массив byte

        public UserStatus Status { get; set; }



        public List<Project> Projects { get; set; } = new List<Project>();    // связь многие ко многим с Projects

        public List<Desk> Desks { get; set; } = new List<Desk>();           // много Users к одному Desk

        public List<Task> Tasks { get; set; } = new List<Task>();            // много Users к одному Task


        public User() { }

        // конструктор, чтобы сразу задать пользователяб статус задан по умолчанию

        public User(string sname, string name, string email, string pass, string phone = null,
            UserStatus status = UserStatus.User, byte[] photo = null)
        {
            Surname = sname;
            Name = name;
            Email = email;
            Password = pass;
            Phone = phone;
            Status = status;
            Photo = photo;
            RegistrationDate = DateTime.Now;

        }

        // применим патерн DTO
        public UserModel ToDto()
        {
            return new UserModel()
            {
                Id = this.Id,
                Surname = this.Surname,
                Name = this.Name,
                Email = this.Email,
                Password = this.Password,
                Phone = this.Phone,
                Status = this.Status,
                Photo = this.Photo,
                RegistrationDate = this.RegistrationDate

            };
        }
    }

}

