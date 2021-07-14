using Microsoft.EntityFrameworkCore.Migrations;

namespace DasTeamRevolution.Migrations
{
    public partial class RenameAllTablesAndColumnsToMatchDbSchemaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_CustomerUser_CustomerUserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeRecord_CustomerUser_CustomerUserId",
                table: "TimeRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_Customer_CustomerId",
                table: "Vacancy");

            migrationBuilder.DropTable(
                name: "CustomerHeaderAdmin");

            migrationBuilder.DropTable(
                name: "CustomerProfile");

            migrationBuilder.DropTable(
                name: "CustomerSupplier");

            migrationBuilder.DropTable(
                name: "CustomerUserPermission");

            migrationBuilder.DropTable(
                name: "DossierState");

            migrationBuilder.DropTable(
                name: "SupplierUserPermission");

            migrationBuilder.DropTable(
                name: "TimeRecordState");

            migrationBuilder.DropTable(
                name: "VacancyState");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerUser");

            migrationBuilder.DropTable(
                name: "Dossier");

            migrationBuilder.DropTable(
                name: "CustomerGroup");

            migrationBuilder.DropTable(
                name: "CustomerHeader");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Vacancy",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacancy_CustomerId",
                table: "Vacancy",
                newName: "IX_Vacancy_ClientId");

            migrationBuilder.RenameColumn(
                name: "CustomerUserId",
                table: "TimeRecord",
                newName: "ClientUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeRecord_CustomerUserId",
                table: "TimeRecord",
                newName: "IX_TimeRecord_ClientUserId");

            migrationBuilder.RenameColumn(
                name: "CustomerUserId",
                table: "Rating",
                newName: "ClientUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_CustomerUserId",
                table: "Rating",
                newName: "IX_Rating_ClientUserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Order",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                newName: "IX_Order_ClientId");

            migrationBuilder.CreateTable(
                name: "ClientHeader",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientHeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUser_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proposal",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierUserId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    VacancyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proposal_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Proposal_SupplierUser_SupplierUserId",
                        column: x => x.SupplierUserId,
                        principalTable: "SupplierUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Proposal_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierUserSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierUserSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierUserSetting_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierUserSetting_SupplierUser_SupplierUserId",
                        column: x => x.SupplierUserId,
                        principalTable: "SupplierUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeRecordStateHistory",
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
                    table.PrimaryKey("PK_TimeRecordStateHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeRecordStateHistory_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeRecordStateHistory_TimeRecord_TimeRecordId",
                        column: x => x.TimeRecordId,
                        principalTable: "TimeRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VacancyStateHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateType = table.Column<int>(type: "int", nullable: false),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    VacancyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyStateHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacancyStateHistory_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VacancyStateHistory_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    HeaderId = table.Column<long>(type: "bigint", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientGroup_ClientGroup_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ClientGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientGroup_ClientHeader_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "ClientHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientGroup_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientGroup_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientHeaderAdmin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    AdminId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientHeaderAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientHeaderAdmin_ApplicationUser_AdminId",
                        column: x => x.AdminId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientHeaderAdmin_ClientHeader_ClientHeaderId",
                        column: x => x.ClientHeaderId,
                        principalTable: "ClientHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposalStateHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateType = table.Column<int>(type: "int", nullable: false),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    ProposalId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalStateHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposalStateHistory_Proposal_ProposalId",
                        column: x => x.ProposalId,
                        principalTable: "Proposal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProposalStateHistory_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client",
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
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_ClientGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ClientGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_PostalAddress_PostalAddressId",
                        column: x => x.PostalAddressId,
                        principalTable: "PostalAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientEmployeeProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEmployeeProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientEmployeeProfile_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientEmployeeProfile_EmployeeProfile_EmployeeProfileId",
                        column: x => x.EmployeeProfileId,
                        principalTable: "EmployeeProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSupplier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSupplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSupplier_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientSupplier_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUserSetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    ClientUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserSetting_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientUserSetting_ClientUser_ClientUserId",
                        column: x => x.ClientUserId,
                        principalTable: "ClientUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_CreationId",
                table: "Client",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_GroupId",
                table: "Client",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_LastModificationId",
                table: "Client",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PostalAddressId",
                table: "Client",
                column: "PostalAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEmployeeProfile_ClientId",
                table: "ClientEmployeeProfile",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEmployeeProfile_EmployeeProfileId",
                table: "ClientEmployeeProfile",
                column: "EmployeeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGroup_CreationId",
                table: "ClientGroup",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGroup_HeaderId",
                table: "ClientGroup",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGroup_LastModificationId",
                table: "ClientGroup",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGroup_ParentId",
                table: "ClientGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientHeaderAdmin_AdminId",
                table: "ClientHeaderAdmin",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientHeaderAdmin_ClientHeaderId",
                table: "ClientHeaderAdmin",
                column: "ClientHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSupplier_ClientId",
                table: "ClientSupplier",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSupplier_SupplierId",
                table: "ClientSupplier",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_ApplicationUserId",
                table: "ClientUser",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserSetting_ClientId",
                table: "ClientUserSetting",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserSetting_ClientUserId",
                table: "ClientUserSetting",
                column: "ClientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_EmployeeId",
                table: "Proposal",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_SupplierUserId",
                table: "Proposal",
                column: "SupplierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Proposal_VacancyId",
                table: "Proposal",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalStateHistory_CreationId",
                table: "ProposalStateHistory",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalStateHistory_ProposalId",
                table: "ProposalStateHistory",
                column: "ProposalId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierUserSetting_SupplierId",
                table: "SupplierUserSetting",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierUserSetting_SupplierUserId",
                table: "SupplierUserSetting",
                column: "SupplierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecordStateHistory_CreationId",
                table: "TimeRecordStateHistory",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecordStateHistory_TimeRecordId",
                table: "TimeRecordStateHistory",
                column: "TimeRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyStateHistory_CreationId",
                table: "VacancyStateHistory",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyStateHistory_VacancyId",
                table: "VacancyStateHistory",
                column: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Client_ClientId",
                table: "Order",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_ClientUser_ClientUserId",
                table: "Rating",
                column: "ClientUserId",
                principalTable: "ClientUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRecord_ClientUser_ClientUserId",
                table: "TimeRecord",
                column: "ClientUserId",
                principalTable: "ClientUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_Client_ClientId",
                table: "Vacancy",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Client_ClientId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_ClientUser_ClientUserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeRecord_ClientUser_ClientUserId",
                table: "TimeRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancy_Client_ClientId",
                table: "Vacancy");

            migrationBuilder.DropTable(
                name: "ClientEmployeeProfile");

            migrationBuilder.DropTable(
                name: "ClientHeaderAdmin");

            migrationBuilder.DropTable(
                name: "ClientSupplier");

            migrationBuilder.DropTable(
                name: "ClientUserSetting");

            migrationBuilder.DropTable(
                name: "ProposalStateHistory");

            migrationBuilder.DropTable(
                name: "SupplierUserSetting");

            migrationBuilder.DropTable(
                name: "TimeRecordStateHistory");

            migrationBuilder.DropTable(
                name: "VacancyStateHistory");

            migrationBuilder.DropTable(
                name: "EmployeeProfile");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "ClientUser");

            migrationBuilder.DropTable(
                name: "Proposal");

            migrationBuilder.DropTable(
                name: "ClientGroup");

            migrationBuilder.DropTable(
                name: "ClientHeader");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Vacancy",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacancy_ClientId",
                table: "Vacancy",
                newName: "IX_Vacancy_CustomerId");

            migrationBuilder.RenameColumn(
                name: "ClientUserId",
                table: "TimeRecord",
                newName: "CustomerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeRecord_ClientUserId",
                table: "TimeRecord",
                newName: "IX_TimeRecord_CustomerUserId");

            migrationBuilder.RenameColumn(
                name: "ClientUserId",
                table: "Rating",
                newName: "CustomerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_ClientUserId",
                table: "Rating",
                newName: "IX_Rating_CustomerUserId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Order",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_ClientId",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.CreateTable(
                name: "CustomerHeader",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHeader", x => x.Id);
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
                name: "Dossier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    SupplierUserId = table.Column<long>(type: "bigint", nullable: true),
                    VacancyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dossier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dossier_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dossier_SupplierUser_SupplierUserId",
                        column: x => x.SupplierUserId,
                        principalTable: "SupplierUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dossier_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierUserPermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierUserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierUserPermission_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierUserPermission_SupplierUser_SupplierUserId",
                        column: x => x.SupplierUserId,
                        principalTable: "SupplierUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeRecordState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    StateType = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VacancyState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    StateType = table.Column<int>(type: "int", nullable: false),
                    VacancyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancyState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacancyState_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VacancyState_Vacancy_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    HeaderId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGroup_CustomerGroup_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerGroup_CustomerHeader_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "CustomerHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerGroup_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerGroup_RecordModification_LastModificationId",
                        column: x => x.LastModificationId,
                        principalTable: "RecordModification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerHeaderAdmin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerHeaderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHeaderAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerHeaderAdmin_ApplicationUser_AdminId",
                        column: x => x.AdminId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerHeaderAdmin_CustomerHeader_CustomerHeaderId",
                        column: x => x.CustomerHeaderId,
                        principalTable: "CustomerHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DossierState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    DossierId = table.Column<long>(type: "bigint", nullable: true),
                    StateType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DossierState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DossierState_Dossier_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DossierState_RecordCreation_CreationId",
                        column: x => x.CreationId,
                        principalTable: "RecordCreation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationId = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationId = table.Column<long>(type: "bigint", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalAddressId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "CustomerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_PostalAddress_PostalAddressId",
                        column: x => x.PostalAddressId,
                        principalTable: "PostalAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "CustomerSupplier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSupplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSupplier_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSupplier_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_CustomerGroup_CreationId",
                table: "CustomerGroup",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroup_HeaderId",
                table: "CustomerGroup",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroup_LastModificationId",
                table: "CustomerGroup",
                column: "LastModificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGroup_ParentId",
                table: "CustomerGroup",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerHeaderAdmin_AdminId",
                table: "CustomerHeaderAdmin",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerHeaderAdmin_CustomerHeaderId",
                table: "CustomerHeaderAdmin",
                column: "CustomerHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfile_CustomerId",
                table: "CustomerProfile",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfile_ProfileId",
                table: "CustomerProfile",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupplier_CustomerId",
                table: "CustomerSupplier",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSupplier_SupplierId",
                table: "CustomerSupplier",
                column: "SupplierId");

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
                name: "IX_DossierState_CreationId",
                table: "DossierState",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_DossierState_DossierId",
                table: "DossierState",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierUserPermission_SupplierId",
                table: "SupplierUserPermission",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierUserPermission_SupplierUserId",
                table: "SupplierUserPermission",
                column: "SupplierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecordState_CreationId",
                table: "TimeRecordState",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecordState_TimeRecordId",
                table: "TimeRecordState",
                column: "TimeRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyState_CreationId",
                table: "VacancyState",
                column: "CreationId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancyState_VacancyId",
                table: "VacancyState",
                column: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_CustomerUser_CustomerUserId",
                table: "Rating",
                column: "CustomerUserId",
                principalTable: "CustomerUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRecord_CustomerUser_CustomerUserId",
                table: "TimeRecord",
                column: "CustomerUserId",
                principalTable: "CustomerUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancy_Customer_CustomerId",
                table: "Vacancy",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
