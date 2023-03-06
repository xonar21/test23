using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_ModifyOnDeleteBehaviourForEmailList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Contacts_ContactId",
                schema: "Crm",
                table: "Emails",
                column: "ContactId",
                principalSchema: "Crm",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Leads_LeadId",
                schema: "Crm",
                table: "Emails",
                column: "LeadId",
                principalSchema: "Crm",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_Organizations_OrganizationId",
                schema: "Crm",
                table: "Emails",
                column: "OrganizationId",
                principalSchema: "Crm",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
    }
}
