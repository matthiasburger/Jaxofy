using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedCustomerHeaderAdminJunctionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerHeaderAdmin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    AdminId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHeaderAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerHeaderAdmin_ApplicationUser_AdminId",
                        column: x => x.AdminId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerHeaderAdmin_CustomerHeader_CustomerHeaderId",
                        column: x => x.CustomerHeaderId,
                        principalTable: "CustomerHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerHeaderAdmin_AdminId",
                table: "CustomerHeaderAdmin",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerHeaderAdmin_CustomerHeaderId",
                table: "CustomerHeaderAdmin",
                column: "CustomerHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerHeaderAdmin");
        }
    }
}
