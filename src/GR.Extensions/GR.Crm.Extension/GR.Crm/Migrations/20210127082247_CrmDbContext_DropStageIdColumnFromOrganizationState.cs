using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_DropStageIdColumnFromOrganizationState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates",
                nullable: true);

        }
    }
}
