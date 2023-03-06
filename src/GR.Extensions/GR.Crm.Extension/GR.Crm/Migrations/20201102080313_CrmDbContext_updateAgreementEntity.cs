using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_updateAgreementEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Contacts_ContactId",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Leads_LeadId",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "Commission",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                schema: "Crm",
                table: "Agreements",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                schema: "Crm",
                table: "Agreements",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "Crm",
                table: "Agreements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_CurrencyCode",
                schema: "Crm",
                table: "Agreements",
                column: "CurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Contacts_ContactId",
                schema: "Crm",
                table: "Agreements",
                column: "ContactId",
                principalSchema: "Crm",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Currencies_CurrencyCode",
                schema: "Crm",
                table: "Agreements",
                column: "CurrencyCode",
                principalSchema: "Crm",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Leads_LeadId",
                schema: "Crm",
                table: "Agreements",
                column: "LeadId",
                principalSchema: "Crm",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Contacts_ContactId",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Currencies_CurrencyCode",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Leads_LeadId",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_CurrencyCode",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "Crm",
                table: "Agreements");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                schema: "Crm",
                table: "Agreements",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                schema: "Crm",
                table: "Agreements",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Commission",
                schema: "Crm",
                table: "Agreements",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Contacts_ContactId",
                schema: "Crm",
                table: "Agreements",
                column: "ContactId",
                principalSchema: "Crm",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Leads_LeadId",
                schema: "Crm",
                table: "Agreements",
                column: "LeadId",
                principalSchema: "Crm",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
