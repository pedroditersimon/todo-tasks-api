using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TodoAPI.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationalTodoGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTask_TodoGoal_TodoGoalID",
                table: "TodoTask");

            migrationBuilder.DropIndex(
                name: "IX_TodoTask_TodoGoalID",
                table: "TodoTask");

            migrationBuilder.DropColumn(
                name: "TodoGoalID",
                table: "TodoTask");

            migrationBuilder.CreateTable(
                name: "TodoTaskGoal",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TodoTaskID = table.Column<int>(type: "integer", nullable: false),
                    TodoGoalID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTaskGoal", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TodoTaskGoal_TodoGoal_TodoGoalID",
                        column: x => x.TodoGoalID,
                        principalTable: "TodoGoal",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoTaskGoal_TodoTask_TodoTaskID",
                        column: x => x.TodoTaskID,
                        principalTable: "TodoTask",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoTaskGoal_TodoGoalID",
                table: "TodoTaskGoal",
                column: "TodoGoalID");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTaskGoal_TodoTaskID",
                table: "TodoTaskGoal",
                column: "TodoTaskID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoTaskGoal");

            migrationBuilder.AddColumn<int>(
                name: "TodoGoalID",
                table: "TodoTask",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 1,
                column: "TodoGoalID",
                value: null);

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 2,
                column: "TodoGoalID",
                value: null);

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 3,
                column: "TodoGoalID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_TodoTask_TodoGoalID",
                table: "TodoTask",
                column: "TodoGoalID");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTask_TodoGoal_TodoGoalID",
                table: "TodoTask",
                column: "TodoGoalID",
                principalTable: "TodoGoal",
                principalColumn: "ID");
        }
    }
}
