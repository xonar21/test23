using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_RemoveLeadReferenceFromEmailList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_Leads_LeadId",
                schema: "Crm",
                table: "Emails");

            migrationBuilder.DropIndex(
                name: "IX_Emails_LeadId",
                schema: "Crm",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "LeadId",
                schema: "Crm",
                table: "Emails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LeadId",
                schema: "Crm",
                table: "Emails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emails_LeadId",
                schema: "Crm",
                table: "Emails",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Leads_LeadId",
                schema: "Crm",
                table: "Emails",
                column: "LeadId",
                principalSchema: "Crm",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
