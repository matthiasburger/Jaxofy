using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class RenamedTablesProfileAndBranchProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchProfiles_Branch_BranchId",
                table: "BranchProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_BranchProfiles_Profiles_ProfileId",
                table: "BranchProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BranchProfiles",
                table: "BranchProfiles");

            migrationBuilder.RenameTable(
                name: "Profiles",
                newName: "Profile");

            migrationBuilder.RenameTable(
                name: "BranchProfiles",
                newName: "BranchProfile");

            migrationBuilder.RenameIndex(
                name: "IX_BranchProfiles_ProfileId",
                table: "BranchProfile",
                newName: "IX_BranchProfile_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_BranchProfiles_BranchId",
                table: "BranchProfile",
                newName: "IX_BranchProfile_BranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profile",
                table: "Profile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BranchProfile",
                table: "BranchProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchProfile_Branch_BranchId",
                table: "BranchProfile",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BranchProfile_Profile_ProfileId",
                table: "BranchProfile",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchProfile_Branch_BranchId",
                table: "BranchProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_BranchProfile_Profile_ProfileId",
                table: "BranchProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profile",
                table: "Profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BranchProfile",
                table: "BranchProfile");

            migrationBuilder.RenameTable(
                name: "Profile",
                newName: "Profiles");

            migrationBuilder.RenameTable(
                name: "BranchProfile",
                newName: "BranchProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_BranchProfile_ProfileId",
                table: "BranchProfiles",
                newName: "IX_BranchProfiles_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_BranchProfile_BranchId",
                table: "BranchProfiles",
                newName: "IX_BranchProfiles_BranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BranchProfiles",
                table: "BranchProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchProfiles_Branch_BranchId",
                table: "BranchProfiles",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BranchProfiles_Profiles_ProfileId",
                table: "BranchProfiles",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
