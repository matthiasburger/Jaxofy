using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class ModifyColumnTypeUserChangePassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangePassword",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<DateTime>(
                name: "NewPasswordRequiredOn",
                table: "ApplicationUser",
                type: "date",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewPasswordRequiredOn",
                table: "ApplicationUser");

            migrationBuilder.AddColumn<bool>(
                name: "ChangePassword",
                table: "ApplicationUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
