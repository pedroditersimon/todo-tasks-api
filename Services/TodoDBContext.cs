using Microsoft.EntityFrameworkCore;
using TodoAPI.Migrations.Seeds;
using TodoAPI.Models;

namespace TodoAPI.Services;

public class TodoDBContext : DbContext
{
    public DbSet<TodoTask> Tasks { get; set; }
    public DbSet<TodoGoal> Goals { get; set; }

    public TodoDBContext(DbContextOptions<TodoDBContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        /*
        // Create database with this initial values (seed)
        modelBuilder.Entity<TodoTask>().HasData(
            new TodoTask() { ID = 1, Name = "my First Task", Description = "Inserted by ef migrations" },
            new TodoTask() { ID = 2, Name = "TodoTask1", Description = "ModelBuilder builder" }
        );
        */

        /*
        // Handling seeds with a class (constructor)
        new TodoTaskSeed(modelBuilder);
        */

        // Handling seeds with IEntityTypeConfiguration
        modelBuilder.ApplyConfiguration(new TodoTaskSeed());

        // dont include Soft deleted task in any queries
        modelBuilder.Entity<TodoTask>().HasQueryFilter(t => !t.IsDeleted);
    }

}
