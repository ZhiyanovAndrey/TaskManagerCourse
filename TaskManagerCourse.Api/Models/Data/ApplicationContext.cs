using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using TaskManager.Common.Models;

namespace TaskManagerCourse.Api.Models.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } // прописываем каждый класс для связи
        public DbSet<ProjectAdmin> ProjectAdmins { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Task> Tasks { get; set; }

        // принимает опции передадим их в базовый класс 
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
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


    }
}
