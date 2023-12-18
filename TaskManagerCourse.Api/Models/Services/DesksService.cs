using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Models;
using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Models.Services
{
    public class DesksService : AbstractionService, ICommonServiсe<DeskModel>
    {
        private readonly ApplicationContext _db;

        // конструктор класса
        public DesksService(ApplicationContext db)
        {
            _db = db;

        }

        public bool Create(DeskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Desk newDeck = new Desk(model);
                _db.Desks.Add(newDeck);
                _db.SaveChanges();

            });
            return result;
            /* Desk newDeck = new Desk(model);
             _db.Desks.Add(newDeck); оборачиваем эту часть в DoAction*/
        }

        public bool Delete(int id)
        {
            bool result = DoAction(delegate ()
            {
                Desk newDeck = _db.Desks.FirstOrDefault(d=>d.Id==id);
                _db.Desks.Remove(newDeck);
                _db.SaveChanges();

            });
            return result;
        }

        public DeskModel Get(int id)
        {
            Desk desk = _db.Desks.Include(d => d.Tasks).FirstOrDefault(d => d.Id == id); // по id мы получаем расширенное представление модели поэтому включаем Tasks
            var deskModel = desk?.ToDto();
            if (deskModel != null)
                deskModel.TasksIds = desk?.Tasks.Select(t => t.Id).ToList(); // проверяем  desk на null


            return deskModel;
        }

        public bool Update(int id, DeskModel model)
        {
            bool result = DoAction(delegate ()
            {
                Desk desk = _db.Desks.FirstOrDefault(d => d.Id == id);
                desk.Name = model.Name;
                desk.Description = model.Description;
                desk.Photo = model.Photo;
                desk.AdminId = model.AdminId;
                desk.isPrivate = model.isPrivate;
                desk.ProjectId = model.ProjectId;
                desk.Columns = "[" + string.Join(",", model.Columns) + "]";
                _db.Desks.Update(desk);
                _db.SaveChanges();

            });
            return result;
        }

        private bool DoAction(Action action)
        {
            try
            {
                action.Invoke(); // вызываем методы сообщенные с делегатом
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
