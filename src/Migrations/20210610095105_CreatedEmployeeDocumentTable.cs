using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class CreatedEmployeeDocumentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDocument_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeDocument_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeDocument_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocument_CreationId",
                table: "EmployeeDocument",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocument_EmployeeId",
                table: "EmployeeDocument",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocument_LastModificationId",
                table: "EmployeeDocument",
                column: "LastModificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDocument");
        }
    }
}
