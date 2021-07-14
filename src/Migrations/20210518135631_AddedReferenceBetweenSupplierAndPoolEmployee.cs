using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedReferenceBetweenSupplierAndPoolEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "PoolEmployee",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployee_SupplierId",
                table: "PoolEmployee",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoolEmployee_Supplier_SupplierId",
                table: "PoolEmployee",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoolEmployee_Supplier_SupplierId",
                table: "PoolEmployee");

            migrationBuilder.DropIndex(
                name: "IX_PoolEmployee_SupplierId",
                table: "PoolEmployee");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PoolEmployee");
        }
    }
}
