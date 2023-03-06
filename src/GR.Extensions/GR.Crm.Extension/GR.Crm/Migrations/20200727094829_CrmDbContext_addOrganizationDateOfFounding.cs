using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_addOrganizationDateOfFounding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfFounding",
                schema: "Crm",
                table: "Organizations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfFounding",
                schema: "Crm",
                table: "Organizations");
        }
    }
}
