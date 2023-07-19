namespace TaskManagerCourse.Api.Abstractions
{
    public interface ICommonServiсe<T>
    {
       // определяет создался обьект или нет
        bool Create(T model);
        bool Update(int id, T model);
        bool Delete(int id);
    }
}
