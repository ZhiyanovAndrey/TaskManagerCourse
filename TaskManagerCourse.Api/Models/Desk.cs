using TaskManager.Common.Models;

namespace TaskManagerCourse.Api.Models
{
    public class Desk : CommonObject
    {
        public int Id { get; set; }

        public bool isPrivate { get; set; }

        public string Columns { get; set; } // хранение в json формате

        //  Доска принадлежит Admin, он  может быть только один
        public int AdminId { get; set; }
        public User Admin { get; set; }


        // Доска принадлежит проекту, покажем БД что это ключ на Project ниже
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        // одной доске принадлежит много задач 
        public List<Task> Tasks { get; set; } = new List<Task>();

        // создаем конструктор метод create вложет сюда модель, EF Core ориентируется на пустой конструктор 

        public Desk() { }

        public Desk(DeskModel deskModel) : base(deskModel) // наследуемся от конструктора базового класса Project и все свойства от CommonObject передаду
        {
            Id = deskModel.Id;
            AdminId = deskModel.AdminId;
            isPrivate = deskModel.isPrivate;
            AdminId = deskModel.AdminId;
            ProjectId = deskModel.ProjectId;

            if (deskModel.Columns.Any() == null) // если колонка содержит хоть что то
            {
                Columns = "[" + string.Join(",", deskModel.Columns) + "]";
            }

        }

        public DeskModel ToDto()
        {
            return new DeskModel()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Photo = this.Photo,
                CreationDate = this.CreationDate,
                AdminId = this.AdminId,
                isPrivate = this.isPrivate,
                ProjectId = this.ProjectId,
                Columns = this.Columns?.Replace("[", "").Replace("]", "").Split(",")
                // Columns это массив string  делаем Split или JSON
                // массив хранится в квадратных скобках меняем их на пустоту
            };
        }

    }
}