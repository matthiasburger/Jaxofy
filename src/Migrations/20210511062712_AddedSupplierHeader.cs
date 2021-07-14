using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedSupplierHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "HeaderId",
                table: "Supplier",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupplierHeader",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierHeader", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_HeaderId",
                table: "Supplier",
                column: "HeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_SupplierHeader_HeaderId",
                table: "Supplier",
                column: "HeaderId",
                principalTable: "SupplierHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_SupplierHeader_HeaderId",
                table: "Supplier");

            migrationBuilder.DropTable(
                name: "SupplierHeader");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_HeaderId",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "HeaderId",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Supplier");
        }
    }
}
