using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAPI.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedTaskLastDeletedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastDeletedTime",
                table: "TodoTask",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDeletedTime",
                table: "TodoGoal",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 1,
                column: "LastDeletedTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TodoGoal",
                keyColumn: "ID",
                keyValue: 2,
                column: "LastDeletedTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 1,
                column: "LastDeletedTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 2,
                column: "LastDeletedTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TodoTask",
                keyColumn: "ID",
                keyValue: 3,
                column: "LastDeletedTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDeletedTime",
                table: "TodoTask");

            migrationBuilder.DropColumn(
                name: "LastDeletedTime",
                table: "TodoGoal");
        }
    }
}
