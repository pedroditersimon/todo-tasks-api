using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGenericRepositories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Goals_TodoGoalID",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goals",
                table: "Goals");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TodoTask");

            migrationBuilder.RenameTable(
                name: "Goals",
                newName: "TodoGoal");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TodoGoalID",
                table: "TodoTask",
                newName: "IX_TodoTask_TodoGoalID");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "TodoGoal",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoGoal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTask",
                table: "TodoTask",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoGoal",
                table: "TodoGoal",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTask_TodoGoal_TodoGoalID",
                table: "TodoTask",
                column: "TodoGoalID",
                principalTable: "TodoGoal",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTask_TodoGoal_TodoGoalID",
                table: "TodoTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTask",
                table: "TodoTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoGoal",
                table: "TodoGoal");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "TodoGoal");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoGoal");

            migrationBuilder.RenameTable(
                name: "TodoTask",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "TodoGoal",
                newName: "Goals");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTask_TodoGoalID",
                table: "Tasks",
                newName: "IX_Tasks_TodoGoalID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goals",
                table: "Goals",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Goals_TodoGoalID",
                table: "Tasks",
                column: "TodoGoalID",
                principalTable: "Goals",
                principalColumn: "ID");
        }
    }
}
