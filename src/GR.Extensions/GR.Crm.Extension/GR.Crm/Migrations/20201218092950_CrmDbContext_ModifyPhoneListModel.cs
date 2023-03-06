using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_ModifyPhoneListModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "Crm",
                table: "PhoneLists",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "Crm",
                table: "PhoneLists",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DialCode",
                schema: "Crm",
                table: "PhoneLists",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "Crm",
                table: "PhoneLists");

            migrationBuilder.DropColumn(
                name: "DialCode",
                schema: "Crm",
                table: "PhoneLists");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "Crm",
                table: "PhoneLists",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
