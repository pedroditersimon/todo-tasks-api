using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedPercentToGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CompletedPercent",
                table: "TodoGoal",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 1,
                column: "CompletedPercent",
                value: 0f);

            migrationBuilder.UpdateData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 2,
                column: "CompletedPercent",
                value: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedPercent",
                table: "TodoGoal");
        }
    }
}
