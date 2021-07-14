using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class RenamedTableVacancyStatesToVacancyState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyStates_RecordCreation_CreationId",
                table: "VacancyStates");

            migrationBuilder.DropForeignKey(
                name: "FK_VacancyStates_Vacancy_VacancyId",
                table: "VacancyStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacancyStates",
                table: "VacancyStates");

            migrationBuilder.RenameTable(
                name: "VacancyStates",
                newName: "VacancyState");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyStates_VacancyId",
                table: "VacancyState",
                newName: "IX_VacancyState_VacancyId");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyStates_CreationId",
                table: "VacancyState",
                newName: "IX_VacancyState_CreationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacancyState",
                table: "VacancyState",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyState_RecordCreation_CreationId",
                table: "VacancyState",
                column: "CreationId",
                principalTable: "RecordCreation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyState_Vacancy_VacancyId",
                table: "VacancyState",
                column: "VacancyId",
                principalTable: "Vacancy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacancyState_RecordCreation_CreationId",
                table: "VacancyState");

            migrationBuilder.DropForeignKey(
                name: "FK_VacancyState_Vacancy_VacancyId",
                table: "VacancyState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacancyState",
                table: "VacancyState");

            migrationBuilder.RenameTable(
                name: "VacancyState",
                newName: "VacancyStates");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyState_VacancyId",
                table: "VacancyStates",
                newName: "IX_VacancyStates_VacancyId");

            migrationBuilder.RenameIndex(
                name: "IX_VacancyState_CreationId",
                table: "VacancyStates",
                newName: "IX_VacancyStates_CreationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacancyStates",
                table: "VacancyStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyStates_RecordCreation_CreationId",
                table: "VacancyStates",
                column: "CreationId",
                principalTable: "RecordCreation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VacancyStates_Vacancy_VacancyId",
                table: "VacancyStates",
                column: "VacancyId",
                principalTable: "Vacancy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
