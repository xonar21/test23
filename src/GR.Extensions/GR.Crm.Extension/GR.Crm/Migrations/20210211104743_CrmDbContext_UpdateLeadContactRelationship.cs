using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdateLeadContactRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeadsContacts",
                schema: "Crm",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(nullable: false),
                    LeadId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadsContacts", x => new { x.ContactId, x.LeadId });
                    table.ForeignKey(
                        name: "FK_LeadsContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalSchema: "Crm",
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadsContacts_Leads_LeadId",
                        column: x => x.LeadId,
                        principalSchema: "Crm",
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadsContacts_LeadId",
                schema: "Crm",
                table: "LeadsContacts",
                column: "LeadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadsContacts",
                schema: "Crm");
        }
    }
}
