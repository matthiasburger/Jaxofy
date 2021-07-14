using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class RemovedFKConstraintOnAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Proposal_ProposalId",
                table: "Assignment");

            migrationBuilder.DropIndex(
                name: "IX_Proposal_AssignmentId",
                table: "Proposal");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_ProposalId",
                table: "Assignment");

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_AssignmentId",
                table: "Proposal",
                column: "AssignmentId",
                unique: true,
                filter: "[AssignmentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proposal_AssignmentId",
                table: "Proposal");

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
        }
    }
}
