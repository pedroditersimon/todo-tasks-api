using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalNeedsToUpdateCompletedStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedsToUpdateCompletedStatus",
                table: "TodoGoal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 1,
                column: "NeedsToUpdateCompletedStatus",
                value: false);

            migrationBuilder.UpdateData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 2,
                column: "NeedsToUpdateCompletedStatus",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedsToUpdateCompletedStatus",
                table: "TodoGoal");
        }
    }
}
