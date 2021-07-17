using Microsoft.EntityFrameworkCore.Migrations;

namespace Jaxofy.Migrations
{
    public partial class AddedApplicationUserUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "ApplicationUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Username",
                table: "ApplicationUser",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_Username",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "ApplicationUser");
        }
    }
}
