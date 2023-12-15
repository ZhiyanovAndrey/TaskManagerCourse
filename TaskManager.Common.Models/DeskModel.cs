using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Common.Models
{
    public class DeskModel:CommonModel
    {
     

        public bool isPrivate { get; set; } 

        public string[] Columns { get; set; } // в БД хранится в виде одной строки, в Desk переделаем в масивом вместо json формате

        //  Доска принадлежит Admin, он  может быть только один
        public int AdminId { get; set; }
        


        // Доска принадлежит проекту, покажем БД что это ключ на Project ниже
        public int ProjectId { get; set; }
        

        // задача принадлежит доске
        public List<int> TasksIds { get; set; } // решили возврашать список id вместо public List<TaskModel> TasksIds { get; set; } = new List<TaskModel>();
    }
}
