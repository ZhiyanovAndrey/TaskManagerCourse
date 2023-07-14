using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column(TypeName = "date")] // если вместе с time то ошибка 
        public DateTime RegistrationDate { get; set; }

        [Column(TypeName = "date")] // Чтобы в базе данных был тип только date без time
        public DateTime? LastLoginDate { get; set; }

        public byte[]? Photo { get; set; } // массив byte

        public UserStatus Status { get; set; }



        public List<Project> Projects { get; set; } = new List<Project>();    // связь многие ко многим с Projects

        public List<Desk> Desks { get; set; } = new List<Desk>();           // много Users к одному Desk

        public List<Task> Tasks { get; set; } = new List<Task>();            // много Users к одному Task


        public User() { }


        // конструктор, чтобы сразу задать пользователя статус задан по умолчанию
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

        public User (UserModel model)
        {
           
                Surname = model.Surname;
                Name = model.Name;
                Email = model.Email;
                Password = model.Password;
                Phone = model.Phone;
                Status = model.Status;
                Photo = model.Photo;
                RegistrationDate = model.RegistrationDate;
            
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

