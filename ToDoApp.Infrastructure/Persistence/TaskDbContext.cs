using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain;

namespace ToDoApp.Infrastructure.Persistence;

public class TaskDbContext : DbContext
{
     public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }

    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.HasKey(t => t.ID);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Description).HasMaxLength(500);
            entity.Property(t => t.Status).HasConversion<string>().IsRequired();
            entity.Property(t => t.AssignedTo).HasMaxLength(100);
        });
    }
}