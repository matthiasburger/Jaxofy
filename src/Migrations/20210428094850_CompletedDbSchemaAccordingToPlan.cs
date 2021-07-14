using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class CompletedDbSchemaAccordingToPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_Branch_BranchId",
                table: "Vacancy");

            migrationBuilder.DropTable(
                name: "BranchProfile");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Vacancy",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacancy_BranchId",
                table: "Vacancy",
                newName: "IX_Vacancy_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "RecordModification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "Dossier",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SupplierUserId",
                table: "Dossier",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VacancyId",
                table: "Dossier",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    PostalAddressId = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_PostalAddress_PostalAddressId",
                        column: x => x.PostalAddressId,
                        principalTable: "PostalAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Customer_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerUser_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    PostalAddressId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplier_PostalAddress_PostalAddressId",
                        column: x => x.PostalAddressId,
                        principalTable: "PostalAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Supplier_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Supplier_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierUser_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfile_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProfile_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Order_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerUserPermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerUserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerUserPermission_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerUserPermission_CustomerUser_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "CustomerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignment_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Assignment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerUserId = table.Column<long>(type: "bigint", nullable: true),
                    AssignmentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Assignment_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rating_CustomerUser_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "CustomerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Rating_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rating_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    AssignmentId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeRecord_Assignment_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeRecord_CustomerUser_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "CustomerUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TimeRecord_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeRecord_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeRecordState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateType = table.Column<int>(type: "int", nullable: false),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    TimeRecordId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRecordState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeRecordState_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeRecordState_TimeRecord_TimeRecordId",
                        column: x => x.TimeRecordId,
                        principalTable: "TimeRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dossier_EmployeeId",
                table: "Dossier",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dossier_SupplierUserId",
                table: "Dossier",
                column: "SupplierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dossier_VacancyId",
                table: "Dossier",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_EmployeeId",
                table: "Assignment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_OrderId",
                table: "Assignment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreationId",
                table: "Customer",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_GroupId",
                table: "Customer",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_LastModificationId",
                table: "Customer",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_PostalAddressId",
                table: "Customer",
                column: "PostalAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfile_CustomerId",
                table: "CustomerProfile",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfile_ProfileId",
                table: "CustomerProfile",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUser_ApplicationUserId",
                table: "CustomerUser",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUserPermission_CustomerId",
                table: "CustomerUserPermission",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUserPermission_CustomerUserId",
                table: "CustomerUserPermission",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreationId",
                table: "Order",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_LastModificationId",
                table: "Order",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_AssignmentId",
                table: "Rating",
                column: "AssignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_CreationId",
                table: "Rating",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_CustomerUserId",
                table: "Rating",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_LastModificationId",
                table: "Rating",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_CreationId",
                table: "Supplier",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_LastModificationId",
                table: "Supplier",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_PostalAddressId",
                table: "Supplier",
                column: "PostalAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierUser_ApplicationUserId",
                table: "SupplierUser",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecord_AssignmentId",
                table: "TimeRecord",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecord_CreationId",
                table: "TimeRecord",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecord_CustomerUserId",
                table: "TimeRecord",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecord_LastModificationId",
                table: "TimeRecord",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecordState_CreationId",
                table: "TimeRecordState",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecordState_TimeRecordId",
                table: "TimeRecordState",
                column: "TimeRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dossier_Employee_EmployeeId",
                table: "Dossier",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Dossier_SupplierUser_SupplierUserId",
                table: "Dossier",
                column: "SupplierUserId",
                principalTable: "SupplierUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Dossier_Vacancy_VacancyId",
                table: "Dossier",
                column: "VacancyId",
                principalTable: "Vacancy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_Customer_CustomerId",
                table: "Vacancy",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dossier_Employee_EmployeeId",
                table: "Dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_Dossier_SupplierUser_SupplierUserId",
                table: "Dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_Dossier_Vacancy_VacancyId",
                table: "Dossier");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_Customer_CustomerId",
                table: "Vacancy");

            migrationBuilder.DropTable(
                name: "CustomerProfile");

            migrationBuilder.DropTable(
                name: "CustomerUserPermission");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "SupplierUser");

            migrationBuilder.DropTable(
                name: "TimeRecordState");

            migrationBuilder.DropTable(
                name: "TimeRecord");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "CustomerUser");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Dossier_EmployeeId",
                table: "Dossier");

            migrationBuilder.DropIndex(
                name: "IX_Dossier_SupplierUserId",
                table: "Dossier");

            migrationBuilder.DropIndex(
                name: "IX_Dossier_VacancyId",
                table: "Dossier");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "RecordModification");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Dossier");

            migrationBuilder.DropColumn(
                name: "SupplierUserId",
                table: "Dossier");

            migrationBuilder.DropColumn(
                name: "VacancyId",
                table: "Dossier");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Vacancy",
                newName: "BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacancy_CustomerId",
                table: "Vacancy",
                newName: "IX_Vacancy_BranchId");

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    PostalAddressId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_CustomerGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branch_PostalAddress_PostalAddressId",
                        column: x => x.PostalAddressId,
                        principalTable: "PostalAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branch_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branch_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BranchProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchProfile_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchProfile_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CreationId",
                table: "Branch",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_GroupId",
                table: "Branch",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_LastModificationId",
                table: "Branch",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_PostalAddressId",
                table: "Branch",
                column: "PostalAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchProfile_BranchId",
                table: "BranchProfile",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchProfile_ProfileId",
                table: "BranchProfile",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_Branch_BranchId",
                table: "Vacancy",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
