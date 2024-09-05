using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoAPI.API.Migrations
{
    /// <inheritdoc />
    public partial class NewSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "TodoTask",
                newName: "IsCompleted");

            migrationBuilder.InsertData(
                table: "TodoGoal",
                columns: new[] { "ID", "CreationDate", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hacer un pedido de frutas y verduras frescas", false, "Realizar Pedido a Proveedores" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Crear y promocionar la oferta del día para atraer más clientes", false, "Preparar Oferta del Día" }
                });

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Rutina diaria de ejercicios para mantener la salud", "Hacer Ejercicio" });

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Description", "IsCompleted", "Name" },
                values: new object[] { "Comprar alimentos y productos necesarios para la semana", true, "Hacer la Compra" });

            migrationBuilder.InsertData(
                table: "TodoTask",
                columns: new[] { "ID", "CreationDate", "Description", "IsCompleted", "IsDeleted", "Name", "TodoGoalID" },
                values: new object[] { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Preparar y disfrutar de una cena ligera", false, false, "Cenar", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "TodoTask",
                newName: "Completed");

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Inserted by ef migrations", "my First Task" });

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Completed", "Description", "Name" },
                values: new object[] { false, "ModelBuilder builder", "TodoTask1" });
        }
    }
}
