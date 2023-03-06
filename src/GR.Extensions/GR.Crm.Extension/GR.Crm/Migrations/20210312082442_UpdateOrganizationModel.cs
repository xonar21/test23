using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class UpdateOrganizationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DialCode",
                schema: "Crm",
                table: "Organizations",
                nullable: true,
                defaultValue: "+373");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DialCode",
                schema: "Crm",
                table: "Organizations");
        }
    }
}
