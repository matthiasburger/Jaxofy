using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class AddedColumnsToTimeRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TimeRecord",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountClient",
                table: "TimeRecord",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountTariffFixed",
                table: "TimeRecord",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountWageFixed",
                table: "TimeRecord",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CandidateId",
                table: "TimeRecord",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCenter",
                table: "TimeRecord",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExportedBy",
                table: "TimeRecord",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExportedOn",
                table: "TimeRecord",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MaxHourEntry",
                table: "TimeRecord",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedOn",
                table: "TimeRecord",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReportDocumentId",
                table: "TimeRecord",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShiftId",
                table: "TimeRecord",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WageType",
                table: "TimeRecord",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "AmountClient",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "AmountTariffFixed",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "AmountWageFixed",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "CostCenter",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "ExportedBy",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "ExportedOn",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "MaxHourEntry",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "RemovedOn",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "ReportDocumentId",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "TimeRecord");

            migrationBuilder.DropColumn(
                name: "WageType",
                table: "TimeRecord");
        }
    }
}
