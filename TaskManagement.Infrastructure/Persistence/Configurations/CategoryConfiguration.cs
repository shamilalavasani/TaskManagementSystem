using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        entity.ToTable("Categories");

        entity.HasKey(c => c.Id);

        entity.Property(c => c.Id)
              .IsRequired();

        entity.Property(c => c.Name)
              .IsRequired()
              .HasMaxLength(100);
        entity.HasIndex(c => c.Name).IsUnique();

        entity.Property(c => c.Description)
              .HasMaxLength(500);

        entity.Property(c => c.CreatedAt)
              .IsRequired()
              .HasColumnType("datetime2");

    }
}
