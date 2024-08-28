using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPI.Models;

namespace TodoAPI.Migrations.Seeds;

public class TodoTaskSeed : IEntityTypeConfiguration<TodoTask>
{

    /*
    // Handling seeds with a class (constructor)
    public TodoTaskSeed(ModelBuilder builder)
    {
        builder.Entity<TodoTask>().HasData(
            new TodoTask() { ID = 1, Name = "my First Task", Description = "Inserted by ef migrations" },
            new TodoTask() { ID = 2, Name = "TodoTask1", Description = "ModelBuilder builder" }
        );
    }
    */

    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        // Create database with this initial values (seed)
        builder.HasData(
            new TodoTask() { ID = 1, Name = "my First Task", Description = "Inserted by ef migrations" },
            new TodoTask() { ID = 2, Name = "TodoTask1", Description = "ModelBuilder builder" }
        );
    }
}
