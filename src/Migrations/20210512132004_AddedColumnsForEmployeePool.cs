using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedColumnsForEmployeePool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "PoolEmployee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PoolEmployee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "PoolEmployee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "PoolEmployee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "PoolEmployee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "PoolEmployee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "PoolEmployee");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "PoolEmployee");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "PoolEmployee");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "PoolEmployee");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "PoolEmployee");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "PoolEmployee");
        }
    }
}
