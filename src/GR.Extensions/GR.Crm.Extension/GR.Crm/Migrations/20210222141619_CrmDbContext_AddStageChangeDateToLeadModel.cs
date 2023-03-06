using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddStageChangeDateToLeadModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 22, 14, 16, 18, 573, DateTimeKind.Utc).AddTicks(6567));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads");
        }
    }
}
