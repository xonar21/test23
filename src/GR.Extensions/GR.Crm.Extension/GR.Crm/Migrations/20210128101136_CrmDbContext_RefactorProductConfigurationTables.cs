using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_RefactorProductConfigurationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceConsultancies_ConsultancyVaria~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDesignes_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDevelopements_DevelopmentVaria~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceQAs_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceQAs",
                schema: "Crm",
                table: "ServiceQAs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceDevelopements",
                schema: "Crm",
                table: "ServiceDevelopements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceDesignes",
                schema: "Crm",
                table: "ServiceDesignes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceConsultancies",
                schema: "Crm",
                table: "ServiceConsultancies");

            migrationBuilder.RenameTable(
                name: "ServiceQAs",
                schema: "Crm",
                newName: "QAVariations",
                newSchema: "Crm");

            migrationBuilder.RenameTable(
                name: "ServiceDevelopements",
                schema: "Crm",
                newName: "DevelopmentVariations",
                newSchema: "Crm");

            migrationBuilder.RenameTable(
                name: "ServiceDesignes",
                schema: "Crm",
                newName: "DesignVariations",
                newSchema: "Crm");

            migrationBuilder.RenameTable(
                name: "ServiceConsultancies",
                schema: "Crm",
                newName: "ConsultancyVariations",
                newSchema: "Crm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QAVariations",
                schema: "Crm",
                table: "QAVariations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevelopmentVariations",
                schema: "Crm",
                table: "DevelopmentVariations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignVariations",
                schema: "Crm",
                table: "DesignVariations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsultancyVariations",
                schema: "Crm",
                table: "ConsultancyVariations",
                column: "Id");

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
                name: "FK_ProductOrServiceLists_DevelopmentVariations_DevelopmentVari~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopmentVariationId",
                principalSchema: "Crm",
                principalTable: "DevelopmentVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_QAVariations_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "QAVariationId",
                principalSchema: "Crm",
                principalTable: "QAVariations",
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
                name: "FK_ProductOrServiceLists_DevelopmentVariations_DevelopmentVari~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_QAVariations_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QAVariations",
                schema: "Crm",
                table: "QAVariations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevelopmentVariations",
                schema: "Crm",
                table: "DevelopmentVariations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignVariations",
                schema: "Crm",
                table: "DesignVariations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsultancyVariations",
                schema: "Crm",
                table: "ConsultancyVariations");

            migrationBuilder.RenameTable(
                name: "QAVariations",
                schema: "Crm",
                newName: "ServiceQAs",
                newSchema: "Crm");

            migrationBuilder.RenameTable(
                name: "DevelopmentVariations",
                schema: "Crm",
                newName: "ServiceDevelopements",
                newSchema: "Crm");

            migrationBuilder.RenameTable(
                name: "DesignVariations",
                schema: "Crm",
                newName: "ServiceDesignes",
                newSchema: "Crm");

            migrationBuilder.RenameTable(
                name: "ConsultancyVariations",
                schema: "Crm",
                newName: "ServiceConsultancies",
                newSchema: "Crm");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceQAs",
                schema: "Crm",
                table: "ServiceQAs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceDevelopements",
                schema: "Crm",
                table: "ServiceDevelopements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceDesignes",
                schema: "Crm",
                table: "ServiceDesignes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceConsultancies",
                schema: "Crm",
                table: "ServiceConsultancies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceConsultancies_ConsultancyVaria~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ConsultancyVariationId",
                principalSchema: "Crm",
                principalTable: "ServiceConsultancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDesignes_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DesignVariationId",
                principalSchema: "Crm",
                principalTable: "ServiceDesignes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDevelopements_DevelopmentVaria~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "DevelopmentVariationId",
                principalSchema: "Crm",
                principalTable: "ServiceDevelopements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceQAs_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "QAVariationId",
                principalSchema: "Crm",
                principalTable: "ServiceQAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
