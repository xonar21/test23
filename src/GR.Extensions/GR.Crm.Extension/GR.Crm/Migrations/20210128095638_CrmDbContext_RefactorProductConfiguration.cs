using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_RefactorProductConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceConsultancies_ServiceConsultan~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDesignes_ServiceDesigneId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDevelopements_ServiceDevelopem~",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrServiceLists_ServiceQAs_ServiceQAId",
                schema: "Crm",
                table: "ProductOrServiceLists");

            migrationBuilder.RenameColumn(
                name: "ServiceQAId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "QAVariationId");

            migrationBuilder.RenameColumn(
                name: "ServiceDevelopementId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "DevelopmentVariationId");

            migrationBuilder.RenameColumn(
                name: "ServiceDesigneId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "DesignVariationId");

            migrationBuilder.RenameColumn(
                name: "ServiceConsultancyId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "ConsultancyVariationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_ServiceQAId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_QAVariationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_ServiceDevelopementId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_DevelopmentVariationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_ServiceDesigneId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_DesignVariationId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_ServiceConsultancyId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_ConsultancyVariationId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "ServiceQAId");

            migrationBuilder.RenameColumn(
                name: "DevelopmentVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "ServiceDevelopementId");

            migrationBuilder.RenameColumn(
                name: "DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "ServiceDesigneId");

            migrationBuilder.RenameColumn(
                name: "ConsultancyVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "ServiceConsultancyId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_QAVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_ServiceQAId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_DevelopmentVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_ServiceDevelopementId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_DesignVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_ServiceDesigneId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOrServiceLists_ConsultancyVariationId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                newName: "IX_ProductOrServiceLists_ServiceConsultancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceConsultancies_ServiceConsultan~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceConsultancyId",
                principalSchema: "Crm",
                principalTable: "ServiceConsultancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDesignes_ServiceDesigneId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceDesigneId",
                principalSchema: "Crm",
                principalTable: "ServiceDesignes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceDevelopements_ServiceDevelopem~",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceDevelopementId",
                principalSchema: "Crm",
                principalTable: "ServiceDevelopements",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrServiceLists_ServiceQAs_ServiceQAId",
                schema: "Crm",
                table: "ProductOrServiceLists",
                column: "ServiceQAId",
                principalSchema: "Crm",
                principalTable: "ServiceQAs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
