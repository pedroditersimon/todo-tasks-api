using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPI.Models;

namespace TodoAPI.Migrations.Seeds;

public class TodoGoalSeed : IEntityTypeConfiguration<TodoGoal>
{

    public void Configure(EntityTypeBuilder<TodoGoal> builder)
    {
        // Create database with this initial values (seed)
        builder.HasData(
            new TodoGoal()
            {
                ID = 1,
                Name = "Realizar Pedido a Proveedores",
                Description = "Hacer un pedido de frutas y verduras frescas"
            },
            new TodoGoal()
            {
                ID = 2,
                Name = "Preparar Oferta del Día",
                Description = "Crear y promocionar la oferta del día para atraer más clientes"
            }
        );

    }
}
