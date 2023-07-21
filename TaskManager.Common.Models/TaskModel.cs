using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Common.Models
{
    public class TaskModel:CommonModel
    {
        

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte[] File { get; set; }

        // одной доске принадлежит много задач
        public int DeskId { get; set; }
       
        public string Column { get; set; }


        public int? CreatorId { get; set; } // может и не быть в проекте допускаем null
     

        public int? ExecuterId { get; set; }
    }
}
