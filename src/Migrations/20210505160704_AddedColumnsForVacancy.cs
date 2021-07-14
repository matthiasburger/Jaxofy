using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedColumnsForVacancy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Vacancy",
                type: "datetime2(0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Vacancy",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Vacancy",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vacancy",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RequiredAmountEmployees",
                table: "Vacancy",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Vacancy",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Vacancy",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "RequiredAmountEmployees",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Vacancy");
        }
    }
}
