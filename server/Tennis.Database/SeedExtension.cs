using Microsoft.EntityFrameworkCore;
using Tennis.Database.Models;
namespace Tennis.Database;
public static class SeedExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasData(new Person
        {
            Id = 1,
            Firstname = "John",
            Lastname = "Doe",
            Age = 28,
        });
        modelBuilder.Entity<Person>().HasData(new Person
        {
            Id = 2,
            Firstname = "Jane",
            Lastname = "Doe",
            Age = 24,
        });
        modelBuilder.Entity<Booking>().HasData(new Booking
        {
            Id = 1,
            Week = 6,
            DayOfWeek = 4,
            Hour = 12,
            PersonId = 1,

        });
        modelBuilder.Entity<Booking>().HasData(new Booking
        {
            Id = 2,
            Week = 6,
            DayOfWeek = 2,
            Hour = 15,
            PersonId = 2,

        });
    }
}