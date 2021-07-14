using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class ExtractedPostalAddressDomainModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCity",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "PostalName",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "PostalStreet",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "PostalZipCode",
                table: "Branch");

            migrationBuilder.AddColumn<long>(
                name: "PostalAddressId",
                table: "Vacancy",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PostalAddressId",
                table: "Branch",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostalAddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostalName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PostalStreet = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PostalZipCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    PostalCity = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CountryCodeISO = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalAddress", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vacancy_PostalAddressId",
                table: "Vacancy",
                column: "PostalAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_PostalAddressId",
                table: "Branch",
                column: "PostalAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PostalAddress_CountryCodeISO",
                table: "PostalAddress",
                column: "CountryCodeISO");

            migrationBuilder.CreateIndex(
                name: "IX_PostalAddress_PostalZipCode_PostalCity",
                table: "PostalAddress",
                columns: new[] { "PostalZipCode", "PostalCity" });

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_PostalAddress_PostalAddressId",
                table: "Branch",
                column: "PostalAddressId",
                principalTable: "PostalAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_PostalAddress_PostalAddressId",
                table: "Vacancy",
                column: "PostalAddressId",
                principalTable: "PostalAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_PostalAddress_PostalAddressId",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_PostalAddress_PostalAddressId",
                table: "Vacancy");

            migrationBuilder.DropTable(
                name: "PostalAddress");

            migrationBuilder.DropIndex(
                name: "IX_Vacancy_PostalAddressId",
                table: "Vacancy");

            migrationBuilder.DropIndex(
                name: "IX_Branch_PostalAddressId",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "PostalAddressId",
                table: "Vacancy");

            migrationBuilder.DropColumn(
                name: "PostalAddressId",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "PostalCity",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalName",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalStreet",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalZipCode",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
