using Domain.Users.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Created)
            .IsRequired();

        builder.Property(e => e.Updated);
        
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Password)
            .IsRequired();

        builder.Property(e => e.LevelAccess)
            .IsRequired();

        builder.Property(e => e.RefreshToken)
            .HasDefaultValue(null)
            .HasMaxLength(200);

        builder.HasIndex(e => e.Username)
            .IsUnique();
    }
}