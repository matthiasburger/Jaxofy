using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class CreatedEmployeeToClientRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "Employee",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ClientId",
                table: "Employee",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Client_ClientId",
                table: "Employee",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Client_ClientId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ClientId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Employee");
        }
    }
}
