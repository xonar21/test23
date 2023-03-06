using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdateLeadModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ProductTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ServiceTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                schema: "Crm",
                table: "Leads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProductTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ServiceTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ProductTypeId",
                principalSchema: "Crm",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_ServiceTypes_ServiceTypeId",
                schema: "Crm",
                table: "Leads",
                column: "ServiceTypeId",
                principalSchema: "Crm",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
