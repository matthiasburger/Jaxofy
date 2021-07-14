using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedDossierDomainModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dossier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dossier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DossierState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateType = table.Column<int>(type: "int", nullable: false),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    DossierId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DossierState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DossierState_Dossier_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DossierState_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DossierState_CreationId",
                table: "DossierState",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_DossierState_DossierId",
                table: "DossierState",
                column: "DossierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DossierState");

            migrationBuilder.DropTable(
                name: "Dossier");
        }
    }
}
