using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_ModifyLeadContactOrganizatioin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Emails_ContactId",
                schema: "Crm",
                table: "Emails",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_LeadId",
                schema: "Crm",
                table: "Emails",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_OrganizationId",
                schema: "Crm",
                table: "Emails",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Contacts_ContactId",
                schema: "Crm",
                table: "Emails",
                column: "ContactId",
                principalSchema: "Crm",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Leads_LeadId",
                schema: "Crm",
                table: "Emails",
                column: "LeadId",
                principalSchema: "Crm",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Organizations_OrganizationId",
                schema: "Crm",
                table: "Emails",
                column: "OrganizationId",
                principalSchema: "Crm",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_Contacts_ContactId",
                schema: "Crm",
                table: "Emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Emails_Leads_LeadId",
                schema: "Crm",
                table: "Emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Emails_Organizations_OrganizationId",
                schema: "Crm",
                table: "Emails");

            migrationBuilder.DropIndex(
                name: "IX_Emails_ContactId",
                schema: "Crm",
                table: "Emails");

            migrationBuilder.DropIndex(
                name: "IX_Emails_LeadId",
                schema: "Crm",
                table: "Emails");

            migrationBuilder.DropIndex(
                name: "IX_Emails_OrganizationId",
                schema: "Crm",
                table: "Emails");
        }
    }
}
