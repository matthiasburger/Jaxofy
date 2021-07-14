using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class CreatedCustomerSupplierAndSupplierUserPermissionJunctionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerSupplier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSupplier_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSupplier_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierUserPermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierUserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierUserPermission_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierUserPermission_SupplierUser_SupplierUserId",
                        column: x => x.SupplierUserId,
                        principalTable: "SupplierUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupplier_CustomerId",
                table: "CustomerSupplier",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupplier_SupplierId",
                table: "CustomerSupplier",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierUserPermission_SupplierId",
                table: "SupplierUserPermission",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierUserPermission_SupplierUserId",
                table: "SupplierUserPermission",
                column: "SupplierUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSupplier");

            migrationBuilder.DropTable(
                name: "SupplierUserPermission");
        }
    }
}
