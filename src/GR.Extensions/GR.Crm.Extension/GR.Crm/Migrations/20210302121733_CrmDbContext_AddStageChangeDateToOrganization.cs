using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddStageChangeDateToOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Organizations",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 2, 12, 17, 32, 508, DateTimeKind.Utc).AddTicks(5324));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 2, 12, 17, 32, 507, DateTimeKind.Utc).AddTicks(1817),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 2, 10, 58, 32, 549, DateTimeKind.Utc).AddTicks(3535));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 2, 10, 58, 32, 549, DateTimeKind.Utc).AddTicks(3535),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 2, 12, 17, 32, 507, DateTimeKind.Utc).AddTicks(1817));
        }
    }
}
