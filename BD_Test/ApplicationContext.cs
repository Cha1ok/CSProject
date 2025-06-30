using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BD_Test
{
    internal class ApplicationContext:DbContext
    {
        public DbSet<Model> DbSet { get; set; } = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=пароль_от_postgres");
        }
    }
}
