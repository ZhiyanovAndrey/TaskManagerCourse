using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Common.Models
{
    // копируем все с User кроме отношений
    public class UserModel
    {
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }


        public DateTime RegistrationDate { get; set; }


        public DateTime LastLoginDate { get; set; }

        public byte[] Photo { get; set; }

        public UserStatus Status { get; set; }



        public UserModel() { }

        // фото пока передаваться не будет

        public UserModel(string sname, string name, string email, string pass, string phone = null,
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
    }
}
