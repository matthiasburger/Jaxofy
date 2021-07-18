using Microsoft.EntityFrameworkCore.Migrations;

namespace Jaxofy.Migrations
{
    public partial class TidiedUpTheTrackClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoadieId",
                table: "Track",
                newName: "SongGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Track_SongGuid",
                table: "Track",
                column: "SongGuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Track_SongGuid",
                table: "Track");

            migrationBuilder.RenameColumn(
                name: "SongGuid",
                table: "Track",
                newName: "RoadieId");
        }
    }
}
