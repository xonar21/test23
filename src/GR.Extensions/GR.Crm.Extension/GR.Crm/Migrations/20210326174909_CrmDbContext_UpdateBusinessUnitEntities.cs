using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdateBusinessUnitEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 26, 15, 8, 41, 486, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 26, 15, 8, 41, 486, DateTimeKind.Utc).AddTicks(4705));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Crm",
                table: "DepartmentTeams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Crm",
                table: "Departments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Crm",
                table: "BusinessUnits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Crm",
                table: "ApplicationUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Crm",
                table: "ApplicationUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Crm",
                table: "DepartmentTeams");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Crm",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Crm",
                table: "BusinessUnits");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Crm",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Crm",
                table: "ApplicationUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Organizations",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 26, 15, 8, 41, 486, DateTimeKind.Utc).AddTicks(9560),
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 26, 15, 8, 41, 486, DateTimeKind.Utc).AddTicks(4705),
                oldClrType: typeof(DateTime));
        }
    }
}
