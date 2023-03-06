using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_addOrganizationStagesAndStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StageId",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                schema: "Crm",
                table: "Organizations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizationStages",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationStates",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    OrganizationStageId = table.Column<Guid>(nullable: false),
                    StateStyleClass = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationStates_OrganizationStages_OrganizationStageId",
                        column: x => x.OrganizationStageId,
                        principalSchema: "Crm",
                        principalTable: "OrganizationStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_StageId",
                schema: "Crm",
                table: "Organizations",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_StateId",
                schema: "Crm",
                table: "Organizations",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationStages_DisplayOrder",
                schema: "Crm",
                table: "OrganizationStages",
                column: "DisplayOrder",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationStates_OrganizationStageId",
                schema: "Crm",
                table: "OrganizationStates",
                column: "OrganizationStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStages_StageId",
                schema: "Crm",
                table: "Organizations",
                column: "StageId",
                principalSchema: "Crm",
                principalTable: "OrganizationStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStages_StageId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStages_StateId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "OrganizationStates",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "OrganizationStages",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_StageId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_StateId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "StageId",
                schema: "Crm",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "StateId",
                schema: "Crm",
                table: "Organizations");
        }
    }
}
