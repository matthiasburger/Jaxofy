using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedPoolEmployeeAndPoolEmployeeDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreationId",
                table: "SupplierHeader",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModificationId",
                table: "SupplierHeader",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PoolEmployee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoolEmployee_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PoolEmployee_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PoolEmployeeDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoolEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolEmployeeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoolEmployeeDocuments_PoolEmployee_PoolEmployeeId",
                        column: x => x.PoolEmployeeId,
                        principalTable: "PoolEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PoolEmployeeDocuments_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PoolEmployeeDocuments_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierHeader_CreationId",
                table: "SupplierHeader",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierHeader_LastModificationId",
                table: "SupplierHeader",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployee_CreationId",
                table: "PoolEmployee",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployee_LastModificationId",
                table: "PoolEmployee",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployeeDocuments_CreationId",
                table: "PoolEmployeeDocuments",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployeeDocuments_LastModificationId",
                table: "PoolEmployeeDocuments",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployeeDocuments_PoolEmployeeId",
                table: "PoolEmployeeDocuments",
                column: "PoolEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierHeader_RecordCreation_CreationId",
                table: "SupplierHeader",
                column: "CreationId",
                principalTable: "RecordCreation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierHeader_RecordModification_LastModificationId",
                table: "SupplierHeader",
                column: "LastModificationId",
                principalTable: "RecordModification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierHeader_RecordCreation_CreationId",
                table: "SupplierHeader");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierHeader_RecordModification_LastModificationId",
                table: "SupplierHeader");

            migrationBuilder.DropTable(
                name: "PoolEmployeeDocuments");

            migrationBuilder.DropTable(
                name: "PoolEmployee");

            migrationBuilder.DropIndex(
                name: "IX_SupplierHeader_CreationId",
                table: "SupplierHeader");

            migrationBuilder.DropIndex(
                name: "IX_SupplierHeader_LastModificationId",
                table: "SupplierHeader");

            migrationBuilder.DropColumn(
                name: "CreationId",
                table: "SupplierHeader");

            migrationBuilder.DropColumn(
                name: "LastModificationId",
                table: "SupplierHeader");
        }
    }
}
