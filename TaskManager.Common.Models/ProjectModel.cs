using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Common.Models
{
    public class ProjectModel:CommonModel
    {
        public ProgectStatus Status { get; set; }
        
        public int? AdminId { get; set; }

        public List<int> AllUsersIds { get; set; } // многие ко многим с User
                                                   //public List<UserModel> AllUsers { get; set; } = new List<UserModel>(); //изменили что бы отображать только id при тестировании API,
                                                   //для отображения всех данных вернемся урок 4.4

        public List<int> AllDesksIds { get; set; }  // приватная доска для пользователя
         //public List<DeskModel> AllDesks { get; set; } = new List<DeskModel>();
    }
}
