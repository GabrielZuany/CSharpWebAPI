using Domain.Employees.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Employees;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Fullname)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Age)
            .IsRequired();

        builder.Property(e => e.Postalcode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Photo);

        builder.Property(e => e.Created)
            .IsRequired();

        builder.Property(e => e.Updated);
        
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasIndex(e => new { e.Fullname });
    }
}