using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_FixOrganizationStateForeignKeyReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStages_StateId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStates_StateId",
                schema: "Crm",
                table: "Organizations",
                column: "StateId",
                principalSchema: "Crm",
                principalTable: "OrganizationStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStates_StateId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStages_StateId",
                schema: "Crm",
                table: "Organizations",
                column: "StateId",
                principalSchema: "Crm",
                principalTable: "OrganizationStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
