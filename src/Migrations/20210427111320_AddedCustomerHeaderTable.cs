using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedCustomerHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HeaderId",
                table: "CustomerGroup",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerHeader",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHeader", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroup_HeaderId",
                table: "CustomerGroup",
                column: "HeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerGroup_CustomerHeader_HeaderId",
                table: "CustomerGroup",
                column: "HeaderId",
                principalTable: "CustomerHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerGroup_CustomerHeader_HeaderId",
                table: "CustomerGroup");

            migrationBuilder.DropTable(
                name: "CustomerHeader");

            migrationBuilder.DropIndex(
                name: "IX_CustomerGroup_HeaderId",
                table: "CustomerGroup");

            migrationBuilder.DropColumn(
                name: "HeaderId",
                table: "CustomerGroup");
        }
    }
}
