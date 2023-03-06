using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddSequenceSchemeForLeads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValueSql: "nextval('\"Crm\".\"OrderNumbers\"')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                schema: "Crm",
                table: "Leads");
        }
    }
}
