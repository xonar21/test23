using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddProductConfigurationsModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Products_ProductId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ProductId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.CreateTable(
                name: "DevelopementFrameworks",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevelopementFrameworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PMFrameworks",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMFrameworks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOrServices",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceConsultancies",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceConsultancies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDesignes",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDesignes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDevelopements",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDevelopements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceQAs",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceQAs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductOrServiceLists",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    LeadId = table.Column<Guid>(nullable: false),
                    ProductOrServiceId = table.Column<Guid>(nullable: false),
                    ProductTypeId = table.Column<Guid>(nullable: true),
                    TechnologyTypeId = table.Column<Guid>(nullable: true),
                    ServiceTypeId = table.Column<Guid>(nullable: true),
                    ServiceDevelopementId = table.Column<Guid>(nullable: true),
                    ServiceConsultancyId = table.Column<Guid>(nullable: true),
                    ServiceQAId = table.Column<Guid>(nullable: true),
                    ServiceDesigneId = table.Column<Guid>(nullable: true),
                    DevelopementFrameworkId = table.Column<Guid>(nullable: true),
                    PMFrameworkId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrServiceLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_DevelopementFrameworks_DevelopementFr~",
                        column: x => x.DevelopementFrameworkId,
                        principalSchema: "Crm",
                        principalTable: "DevelopementFrameworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_Leads_LeadId",
                        column: x => x.LeadId,
                        principalSchema: "Crm",
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_PMFrameworks_PMFrameworkId",
                        column: x => x.PMFrameworkId,
                        principalSchema: "Crm",
                        principalTable: "PMFrameworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_ProductOrServices_ProductOrServiceId",
                        column: x => x.ProductOrServiceId,
                        principalSchema: "Crm",
                        principalTable: "ProductOrServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalSchema: "Crm",
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_ServiceConsultancies_ServiceConsultan~",
                        column: x => x.ServiceConsultancyId,
                        principalSchema: "Crm",
                        principalTable: "ServiceConsultancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_ServiceDesignes_ServiceDesigneId",
                        column: x => x.ServiceDesigneId,
                        principalSchema: "Crm",
                        principalTable: "ServiceDesignes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_ServiceDevelopements_ServiceDevelopem~",
                        column: x => x.ServiceDevelopementId,
                        principalSchema: "Crm",
                        principalTable: "ServiceDevelopements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_ServiceQAs_ServiceQAId",
                        column: x => x.ServiceQAId,
                        principalSchema: "Crm",
                        principalTable: "ServiceQAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalSchema: "Crm",
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrServiceLists_TechnologyTypes_TechnologyTypeId",
                        column: x => x.TechnologyTypeId,
                        principalSchema: "Crm",
                        principalTable: "TechnologyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_DevelopementFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopementFrameworkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_LeadId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "PMFrameworkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ProductOrServiceId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductOrServiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ServiceConsultancyId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceConsultancyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ServiceDesigneId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceDesigneId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ServiceDevelopementId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceDevelopementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ServiceQAId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceQAId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "TechnologyTypeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductOrServiceLists",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "DevelopementFrameworks",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "PMFrameworks",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ProductOrServices",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ServiceConsultancies",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ServiceDesignes",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ServiceDevelopements",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ServiceQAs",
                schema: "Crm");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProductId",
                schema: "Crm",
                table: "Leads",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Products_ProductId",
                schema: "Crm",
                table: "Leads",
                column: "ProductId",
                principalSchema: "Crm",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
