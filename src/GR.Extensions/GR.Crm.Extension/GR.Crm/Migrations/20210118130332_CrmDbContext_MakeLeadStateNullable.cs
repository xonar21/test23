using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_MakeLeadStateNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_States_LeadStateId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadStateId",
                schema: "Crm",
                table: "Leads",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_States_LeadStateId",
                schema: "Crm",
                table: "Leads",
                column: "LeadStateId",
                principalSchema: "Crm",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_States_LeadStateId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadStateId",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_States_LeadStateId",
                schema: "Crm",
                table: "Leads",
                column: "LeadStateId",
                principalSchema: "Crm",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
