using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedBranchVacanciesNavigationProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "Vacancy",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_BranchId",
                table: "Vacancy",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_Branch_BranchId",
                table: "Vacancy",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_Branch_BranchId",
                table: "Vacancy");

            migrationBuilder.DropIndex(
                name: "IX_Vacancy_BranchId",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Vacancy");
        }
    }
}
