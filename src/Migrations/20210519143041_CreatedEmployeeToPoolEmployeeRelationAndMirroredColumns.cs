using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class CreatedEmployeeToPoolEmployeeRelationAndMirroredColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "PoolEmployee",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "PoolEmployee",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreationId",
                table: "Employee",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Employee",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModificationId",
                table: "Employee",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Employee",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PoolEmployeeId",
                table: "Employee",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployee_FirstName",
                table: "PoolEmployee",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_PoolEmployee_LastName",
                table: "PoolEmployee",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CreationId",
                table: "Employee",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_FirstName",
                table: "Employee",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_LastModificationId",
                table: "Employee",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_LastName",
                table: "Employee",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_PoolEmployeeId",
                table: "Employee",
                column: "PoolEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_PoolEmployee_PoolEmployeeId",
                table: "Employee",
                column: "PoolEmployeeId",
                principalTable: "PoolEmployee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_RecordCreation_CreationId",
                table: "Employee",
                column: "CreationId",
                principalTable: "RecordCreation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_RecordModification_LastModificationId",
                table: "Employee",
                column: "LastModificationId",
                principalTable: "RecordModification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_PoolEmployee_PoolEmployeeId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_RecordCreation_CreationId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_RecordModification_LastModificationId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_PoolEmployee_FirstName",
                table: "PoolEmployee");

            migrationBuilder.DropIndex(
                name: "IX_PoolEmployee_LastName",
                table: "PoolEmployee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_CreationId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_FirstName",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_LastModificationId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_LastName",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_PoolEmployeeId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreationId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastModificationId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PoolEmployeeId",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "PoolEmployee",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "PoolEmployee",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
