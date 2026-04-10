using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Persistence.Configurations
{
    internal class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> entity)
        {
            entity.ToTable("TodoItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            //entity.Property(e => e.CompletionStatus).IsRequired();
            entity.Property(e => e.CompletionStatus).HasConversion<string>().IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired().HasColumnType("datetime2");
            entity.Property(e => e.DueDate).IsRequired().HasColumnType("datetime2");
            entity.Property(e => e.Priority).HasConversion<string>().IsRequired();
            entity.Property(e => e.CategoryId).IsRequired();
            entity.HasOne(t => t.Category)
                  .WithMany(c => c.TodoItems)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
