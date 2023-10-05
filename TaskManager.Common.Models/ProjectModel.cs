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

        public List<UserModel> AllUsers { get; set; } = new List<UserModel>(); // многие ко многим с User

        public List<DeskModel> AllDecks { get; set; } = new List<DeskModel>(); // приватная доска для пользователя

    }
}
