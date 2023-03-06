using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_DeleteOrganizationTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientType",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "TargetType",
                schema: "Crm",
                table: "MarketingLists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientType",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetType",
                schema: "Crm",
                table: "MarketingLists",
                nullable: false,
                defaultValue: 0);
        }
    }
}
