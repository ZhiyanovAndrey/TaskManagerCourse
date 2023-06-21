using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace TaskManagerCourse.Api.Models.Data
{
    public class AplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } // прописываем каждый класс для связи
        public DbSet<ProjectAdmin> ProjectAdmins { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Task> Tasks { get; set; }

        // принимает опции передадим их в базовый класс 
        public AplicationContext(DbContextOptions<AplicationContext> options) : base(options) 
        {
            Database.EnsureCreated(); // убедиться что БД создана

            // если в базе нет admin то добавить
            if (Users.Any(u => u.Status == UserStatus.Admin) == false)
            {
                var admin = new User();
                Users.Add(admin);
                SaveChanges();
            }

        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        optionsBuilder.UseNpgsql(Iconfiguration.GetConnectionString("DefaultConnection"));
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=TaskManager;Username=postgres;Password=User1234")
        //        //.LogTo(Console.WriteLine) // можно подсмотреть сформированный EF запрос
        //        ;
        //    /*Строка подключения содержит адрес сервера (параметр Host), порт (Port), 
        //     * название базы данных на сервере (StaffManager),
        //     * имя пользователя в рамках сервера PostgreSQL (Username) и его пароль (Password).*/
        //}
    }
}
