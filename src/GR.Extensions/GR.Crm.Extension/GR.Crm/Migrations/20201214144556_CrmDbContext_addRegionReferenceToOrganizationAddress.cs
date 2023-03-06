using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_addRegionReferenceToOrganizationAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                schema: "Crm",
                table: "OrganizationAddresses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationAddresses_RegionId",
                schema: "Crm",
                table: "OrganizationAddresses",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationAddresses_Regions_RegionId",
                schema: "Crm",
                table: "OrganizationAddresses",
                column: "RegionId",
                principalSchema: "Crm",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationAddresses_Regions_RegionId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationAddresses_RegionId",
                schema: "Crm",
                table: "OrganizationAddresses");

            migrationBuilder.DropColumn(
                name: "RegionId",
                schema: "Crm",
                table: "OrganizationAddresses");
        }
    }
}
