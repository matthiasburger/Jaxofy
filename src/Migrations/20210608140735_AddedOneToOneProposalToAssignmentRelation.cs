using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedOneToOneProposalToAssignmentRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AssignmentId",
                table: "Proposal",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProposalId",
                table: "Assignment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_AssignmentId",
                table: "Proposal",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_ProposalId",
                table: "Assignment",
                column: "ProposalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Proposal_ProposalId",
                table: "Assignment",
                column: "ProposalId",
                principalTable: "Proposal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposal_Assignment_AssignmentId",
                table: "Proposal",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Proposal_ProposalId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposal_Assignment_AssignmentId",
                table: "Proposal");

            migrationBuilder.DropIndex(
                name: "IX_Proposal_AssignmentId",
                table: "Proposal");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_ProposalId",
                table: "Assignment");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Proposal");

            migrationBuilder.DropColumn(
                name: "ProposalId",
                table: "Assignment");
        }
    }
}
