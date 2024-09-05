using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TodoAPI.API.Migrations
{
    /// <inheritdoc />
    public partial class CreatedGoalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoalID",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "ID",
                keyValue: 1,
                column: "GoalID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "ID",
                keyValue: 2,
                column: "GoalID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_GoalID",
                table: "Tasks",
                column: "GoalID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Goals_GoalID",
                table: "Tasks",
                column: "GoalID",
                principalTable: "Goals",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Goals_GoalID",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_GoalID",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "GoalID",
                table: "Tasks");
        }
    }
}
