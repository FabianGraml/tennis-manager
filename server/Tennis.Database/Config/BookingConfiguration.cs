using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tennis.Database.Models;
namespace Tennis.Database.Config;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasOne(x => x.Person)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.PersonId);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(x => x.Week)
            .IsRequired();
        builder.Property(x => x.DayOfWeek)
            .IsRequired();
        builder.Property(x => x.PersonId)
            .IsRequired();
        builder.Property(x => x.Hour)
           .IsRequired();
    }
}