using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Chat.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Rule> Rules { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string userRoleName = "user";

            // добавляем тестовые роли
            Role userRole = new Role { Id = 2, Name = userRoleName };
            // добавляем тестовых пользователей
            User User1 = new User { Id = 1, Email = "policarp@mail.com", Password = "1", RoleId = userRole.Id , MQM = 10};
          
            modelBuilder.Entity<Role>().HasData(new Role[] { userRole });
            modelBuilder.Entity<User>().HasData(new User[] { User1 });
            base.OnModelCreating(modelBuilder);
            
        }
    }
    
   
}