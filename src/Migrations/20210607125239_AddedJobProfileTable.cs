using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedJobProfileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobProfile_ClientSupplier_ClientSupplierId",
                table: "JobProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_JobProfile_JobProfileId",
                table: "Vacancy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobProfile",
                table: "JobProfile");

            migrationBuilder.RenameTable(
                name: "JobProfile",
                newName: "JobProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_JobProfile_ClientSupplierId",
                table: "JobProfiles",
                newName: "IX_JobProfiles_ClientSupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobProfiles",
                table: "JobProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobProfiles_ClientSupplier_ClientSupplierId",
                table: "JobProfiles",
                column: "ClientSupplierId",
                principalTable: "ClientSupplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_JobProfiles_JobProfileId",
                table: "Vacancy",
                column: "JobProfileId",
                principalTable: "JobProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobProfiles_ClientSupplier_ClientSupplierId",
                table: "JobProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_JobProfiles_JobProfileId",
                table: "Vacancy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobProfiles",
                table: "JobProfiles");

            migrationBuilder.RenameTable(
                name: "JobProfiles",
                newName: "JobProfile");

            migrationBuilder.RenameIndex(
                name: "IX_JobProfiles_ClientSupplierId",
                table: "JobProfile",
                newName: "IX_JobProfile_ClientSupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobProfile",
                table: "JobProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobProfile_ClientSupplier_ClientSupplierId",
                table: "JobProfile",
                column: "ClientSupplierId",
                principalTable: "ClientSupplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_JobProfile_JobProfileId",
                table: "Vacancy",
                column: "JobProfileId",
                principalTable: "JobProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
