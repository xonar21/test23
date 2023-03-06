using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_MakeClientTypeNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClientType",
                schema: "Crm",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClientType",
                schema: "Crm",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
