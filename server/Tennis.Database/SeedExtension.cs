using Microsoft.EntityFrameworkCore;
using Tennis.Database.Models;
namespace Tennis.Database;
public static class SeedExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>().HasData(new UserRole
        {
            Id = 1,
            RoleName = "Admin",
        });
        modelBuilder.Entity<UserRole>().HasData(new UserRole
        {
            Id = 2,
            RoleName = "User",
        });
    }
}