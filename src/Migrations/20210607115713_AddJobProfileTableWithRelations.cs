using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddJobProfileTableWithRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "JobProfileId",
                table: "Vacancy",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientSupplierId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Factor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobProfile_ClientSupplier_ClientSupplierId",
                        column: x => x.ClientSupplierId,
                        principalTable: "ClientSupplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_JobProfileId",
                table: "Vacancy",
                column: "JobProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_JobProfile_ClientSupplierId",
                table: "JobProfile",
                column: "ClientSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_JobProfile_JobProfileId",
                table: "Vacancy",
                column: "JobProfileId",
                principalTable: "JobProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_JobProfile_JobProfileId",
                table: "Vacancy");

            migrationBuilder.DropTable(
                name: "JobProfile");

            migrationBuilder.DropIndex(
                name: "IX_Vacancy_JobProfileId",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "JobProfileId",
                table: "Vacancy");
        }
    }
}
