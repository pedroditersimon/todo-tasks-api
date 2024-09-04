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
            new TodoTask()
            {
                ID = 1,
                Name = "Hacer Ejercicio",
                Description = "Rutina diaria de ejercicios para mantener la salud"
            },
            new TodoTask()
            {
                ID = 2,
                Name = "Hacer la Compra",
                Description = "Comprar alimentos y productos necesarios para la semana",
                IsCompleted = true
            },
            new TodoTask()
            {
                ID = 3,
                Name = "Cenar",
                Description = "Preparar y disfrutar de una cena ligera"
            }
        );

    }
}
