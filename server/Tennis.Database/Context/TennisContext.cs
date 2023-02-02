using Microsoft.EntityFrameworkCore;
using Tennis.Database.Models;
namespace Tennis.Database.Context;
public class TennisContext : DbContext
{
    public TennisContext(DbContextOptions<TennisContext> options) : base(options) { }
    public virtual DbSet<Booking>? Bookings { get; set; }
    public virtual DbSet<Person>? Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
        base.OnModelCreating(modelBuilder);
    }
}