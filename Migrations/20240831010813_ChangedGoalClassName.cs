using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedGoalClassName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Goals_GoalID",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "GoalID",
                table: "Tasks",
                newName: "TodoGoalID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_GoalID",
                table: "Tasks",
                newName: "IX_Tasks_TodoGoalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Goals_TodoGoalID",
                table: "Tasks",
                column: "TodoGoalID",
                principalTable: "Goals",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Goals_TodoGoalID",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "TodoGoalID",
                table: "Tasks",
                newName: "GoalID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TodoGoalID",
                table: "Tasks",
                newName: "IX_Tasks_GoalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Goals_GoalID",
                table: "Tasks",
                column: "GoalID",
                principalTable: "Goals",
                principalColumn: "ID");
        }
    }
}
