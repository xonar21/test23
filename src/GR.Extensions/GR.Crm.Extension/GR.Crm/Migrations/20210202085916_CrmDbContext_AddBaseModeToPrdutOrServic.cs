using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddBaseModeToPrdutOrServic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ConsultancyVariations_ConsultancyVari~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_DesignVariations_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_DevelopementFrameworks_DevelopementFr~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_DevelopmentVariations_DevelopmentVari~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_PMFrameworks_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_QAVariations_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ConsultancyVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_DevelopementFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_DevelopmentVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ProductOrServiceId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                schema: "Crm",
                table: "ProductOrServiceLists",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Changed",
                schema: "Crm",
                table: "ProductOrServiceLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "Crm",
                table: "ProductOrServiceLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Crm",
                table: "ProductOrServiceLists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "Crm",
                table: "ProductOrServiceLists",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                schema: "Crm",
                table: "ProductOrServiceLists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ConsultancyVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ConsultancyVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DesignVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_DevelopementFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopementFrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_DevelopmentVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopmentVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "PMFrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ProductOrServiceId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductOrServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "QAVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "TechnologyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ConsultancyVariations_ConsultancyVari~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ConsultancyVariationId",
                principalSchema: "Crm",
                principalTable: "ConsultancyVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_DesignVariations_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DesignVariationId",
                principalSchema: "Crm",
                principalTable: "DesignVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_DevelopementFrameworks_DevelopementFr~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopementFrameworkId",
                principalSchema: "Crm",
                principalTable: "DevelopementFrameworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_DevelopmentVariations_DevelopmentVari~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopmentVariationId",
                principalSchema: "Crm",
                principalTable: "DevelopmentVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_PMFrameworks_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "PMFrameworkId",
                principalSchema: "Crm",
                principalTable: "PMFrameworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductTypeId",
                principalSchema: "Crm",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_QAVariations_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "QAVariationId",
                principalSchema: "Crm",
                principalTable: "QAVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceTypeId",
                principalSchema: "Crm",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "TechnologyTypeId",
                principalSchema: "Crm",
                principalTable: "TechnologyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ConsultancyVariations_ConsultancyVari~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_DesignVariations_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_DevelopementFrameworks_DevelopementFr~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_DevelopmentVariations_DevelopmentVari~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_PMFrameworks_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_QAVariations_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ConsultancyVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_DevelopementFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_DevelopmentVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ProductOrServiceId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrServiceLists_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropColumn(
                name: "Author",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropColumn(
                name: "Changed",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ConsultancyVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ConsultancyVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DesignVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_DevelopementFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopementFrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_DevelopmentVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopmentVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "PMFrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ProductOrServiceId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductOrServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "QAVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrServiceLists_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "TechnologyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ConsultancyVariations_ConsultancyVari~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ConsultancyVariationId",
                principalSchema: "Crm",
                principalTable: "ConsultancyVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_DesignVariations_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DesignVariationId",
                principalSchema: "Crm",
                principalTable: "DesignVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_DevelopementFrameworks_DevelopementFr~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopementFrameworkId",
                principalSchema: "Crm",
                principalTable: "DevelopementFrameworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_DevelopmentVariations_DevelopmentVari~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopmentVariationId",
                principalSchema: "Crm",
                principalTable: "DevelopmentVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_PMFrameworks_PMFrameworkId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "PMFrameworkId",
                principalSchema: "Crm",
                principalTable: "PMFrameworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ProductTypeId",
                principalSchema: "Crm",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_QAVariations_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "QAVariationId",
                principalSchema: "Crm",
                principalTable: "QAVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceTypeId",
                principalSchema: "Crm",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "TechnologyTypeId",
                principalSchema: "Crm",
                principalTable: "TechnologyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
