using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tennis.Database.Models;
namespace Tennis.Database.Config;
public class PersonsConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(x => x.Id)
             .IsRequired()
             .ValueGeneratedOnAdd();
        builder.Property(x => x.Firstname)
            .IsRequired()
            .HasMaxLength(25);
        builder.Property(x => x.Lastname)
            .IsRequired()
            .HasMaxLength(25);
    }
}