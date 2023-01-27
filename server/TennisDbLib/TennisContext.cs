using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDbLib
{
    public class TennisContext : DbContext
    {
        public TennisContext(DbContextOptions<TennisContext> options) : base(options)
        {
            Console.WriteLine("Creating TennisContext with options");
        }
        public TennisContext()
        {
            Console.WriteLine("Creating TennisContext default");
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine($"Db OnConfiguring: IsConfigured={optionsBuilder.IsConfigured}");
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=(LocalDB)\mssqllocaldb;attachdbfilename=D:\Temp\TennisDb.mdf;database=TennisDB;integrated security=True";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
        
    }
}
