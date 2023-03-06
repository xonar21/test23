using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddPhoneList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneLists_Contacts_ContactId",
                schema: "Crm",
                table: "PhoneLists");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                schema: "Crm",
                table: "PhoneLists",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneLists_Contacts_ContactId",
                schema: "Crm",
                table: "PhoneLists",
                column: "ContactId",
                principalSchema: "Crm",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneLists_Contacts_ContactId",
                schema: "Crm",
                table: "PhoneLists");

            migrationBuilder.AlterColumn<Guid>(
                name: "ContactId",
                schema: "Crm",
                table: "PhoneLists",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneLists_Contacts_ContactId",
                schema: "Crm",
                table: "PhoneLists",
                column: "ContactId",
                principalSchema: "Crm",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
