using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedGenderAndPostalAddressToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Gender",
                table: "Employee",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<long>(
                name: "PostalAddressId",
                table: "Employee",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PostalAddressId",
                table: "Employee",
                column: "PostalAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_PostalAddress_PostalAddressId",
                table: "Employee",
                column: "PostalAddressId",
                principalTable: "PostalAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_PostalAddress_PostalAddressId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_PostalAddressId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PostalAddressId",
                table: "Employee");
        }
    }
}
