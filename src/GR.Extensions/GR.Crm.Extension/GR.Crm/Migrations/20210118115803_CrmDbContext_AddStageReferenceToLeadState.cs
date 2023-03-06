using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddStageReferenceToLeadState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeadStateStage",
                schema: "Crm",
                columns: table => new
                {
                    StateId = table.Column<Guid>(nullable: false),
                    StageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadStateStage", x => new { x.StateId, x.StageId });
                    table.ForeignKey(
                        name: "FK_LeadStateStage_Stages_StageId",
                        column: x => x.StageId,
                        principalSchema: "Crm",
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadStateStage_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "Crm",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadStateStage_StageId",
                schema: "Crm",
                table: "LeadStateStage",
                column: "StageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadStateStage",
                schema: "Crm");
        }
    }
}
