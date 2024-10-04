using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Migrations.Seeds;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public class TodoDBContext : DbContext
{

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
        modelBuilder.ApplyConfiguration(new TodoGoalSeed());

        // dont include Soft deleted entities in any queries
        modelBuilder.Entity<TodoTask>().HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<TodoGoal>().HasQueryFilter(t => !t.IsDeleted);
    }

}
