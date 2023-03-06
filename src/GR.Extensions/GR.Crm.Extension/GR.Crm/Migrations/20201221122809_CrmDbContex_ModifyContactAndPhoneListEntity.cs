using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContex_ModifyContactAndPhoneListEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "Crm",
                table: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "Label",
                schema: "Crm",
                table: "PhoneLists",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                schema: "Crm",
                table: "PhoneLists");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "Crm",
                table: "Contacts",
                maxLength: 50,
                nullable: true);
        }
    }
}
