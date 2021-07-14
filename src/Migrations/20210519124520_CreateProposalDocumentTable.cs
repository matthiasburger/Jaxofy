using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class CreateProposalDocumentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Proposal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "PoolEmployeeDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProposalId",
                table: "PoolEmployeeDocuments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProposalDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProposalId = table.Column<long>(type: "bigint", nullable: true),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalDocument_Proposal_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProposalDocument_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProposalDocument_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployeeDocuments_ProposalId",
                table: "PoolEmployeeDocuments",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalDocument_CreationId",
                table: "ProposalDocument",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalDocument_LastModificationId",
                table: "ProposalDocument",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalDocument_ProposalId",
                table: "ProposalDocument",
                column: "ProposalId");

            migrationBuilder.AddForeignKey(
                name: "FK_PoolEmployeeDocuments_Proposal_ProposalId",
                table: "PoolEmployeeDocuments",
                column: "ProposalId",
                principalTable: "Proposal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoolEmployeeDocuments_Proposal_ProposalId",
                table: "PoolEmployeeDocuments");

            migrationBuilder.DropTable(
                name: "ProposalDocument");

            migrationBuilder.DropIndex(
                name: "IX_PoolEmployeeDocuments_ProposalId",
                table: "PoolEmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "PoolEmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "PoolEmployeeDocuments");
        }
    }
}
