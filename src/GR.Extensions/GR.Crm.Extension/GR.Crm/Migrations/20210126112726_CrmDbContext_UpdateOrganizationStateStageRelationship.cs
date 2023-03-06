using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdateOrganizationStateStageRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationStates_OrganizationStages_OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateTable(
                name: "OrganizationStatesStages",
                schema: "Crm",
                columns: table => new
                {
                    StateId = table.Column<Guid>(nullable: false),
                    StageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationStatesStages", x => new { x.StateId, x.StageId });
                    table.ForeignKey(
                        name: "FK_OrganizationStatesStages_OrganizationStages_StageId",
                        column: x => x.StageId,
                        principalSchema: "Crm",
                        principalTable: "OrganizationStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationStatesStages_OrganizationStates_StateId",
                        column: x => x.StateId,
                        principalSchema: "Crm",
                        principalTable: "OrganizationStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationStatesStages_StageId",
                schema: "Crm",
                table: "OrganizationStatesStages",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationStates_OrganizationStages_OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates",
                column: "OrganizationStageId",
                principalSchema: "Crm",
                principalTable: "OrganizationStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationStates_OrganizationStages_OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates");

            migrationBuilder.DropTable(
                name: "OrganizationStatesStages",
                schema: "Crm");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationStates_OrganizationStages_OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates",
                column: "OrganizationStageId",
                principalSchema: "Crm",
                principalTable: "OrganizationStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
