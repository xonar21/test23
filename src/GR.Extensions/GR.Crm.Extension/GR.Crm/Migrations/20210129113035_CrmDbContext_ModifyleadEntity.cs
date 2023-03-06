using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_ModifyleadEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SolutionTypes_SolutionTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_SolutionTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_TechnologyTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SolutionTypeId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TechnologyTypeId",
                schema: "Crm",
                table: "Leads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SolutionTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TechnologyTypeId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SolutionTypeId",
                schema: "Crm",
                table: "Leads",
                column: "SolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_TechnologyTypeId",
                schema: "Crm",
                table: "Leads",
                column: "TechnologyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SolutionTypes_SolutionTypeId",
                schema: "Crm",
                table: "Leads",
                column: "SolutionTypeId",
                principalSchema: "Crm",
                principalTable: "SolutionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_TechnologyTypes_TechnologyTypeId",
                schema: "Crm",
                table: "Leads",
                column: "TechnologyTypeId",
                principalSchema: "Crm",
                principalTable: "TechnologyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
