using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedProfileAndBranchProfileJunctionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "CustomerGroup",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchProfiles_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchProfiles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroup_ParentId",
                table: "CustomerGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchProfiles_BranchId",
                table: "BranchProfiles",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchProfiles_ProfileId",
                table: "BranchProfiles",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGroup_CustomerGroup_ParentId",
                table: "CustomerGroup",
                column: "ParentId",
                principalTable: "CustomerGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGroup_CustomerGroup_ParentId",
                table: "CustomerGroup");

            migrationBuilder.DropTable(
                name: "BranchProfiles");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_CustomerGroup_ParentId",
                table: "CustomerGroup");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CustomerGroup");
        }
    }
}
