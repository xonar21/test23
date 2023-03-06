using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_addMarketingList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_CampaignTypes_CampaignTypeId",
                schema: "Crm",
                table: "Campaign");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaign",
                schema: "Crm",
                table: "Campaign");

            migrationBuilder.RenameTable(
                name: "Campaign",
                schema: "Crm",
                newName: "Campaigns",
                newSchema: "Crm");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_CampaignTypeId",
                schema: "Crm",
                table: "Campaigns",
                newName: "IX_Campaigns_CampaignTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns",
                schema: "Crm",
                table: "Campaigns",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MarketingLists",
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
                    Name = table.Column<string>(nullable: false),
                    TargetType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketingLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketingListOrganizations",
                schema: "Crm",
                columns: table => new
                {
                    MarketingListId = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketingListOrganizations", x => new { x.MarketingListId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_MarketingListOrganizations_MarketingLists_MarketingListId",
                        column: x => x.MarketingListId,
                        principalSchema: "Crm",
                        principalTable: "MarketingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarketingListOrganizations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Crm",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketingListOrganizations_OrganizationId",
                schema: "Crm",
                table: "MarketingListOrganizations",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_CampaignTypes_CampaignTypeId",
                schema: "Crm",
                table: "Campaigns",
                column: "CampaignTypeId",
                principalSchema: "Crm",
                principalTable: "CampaignTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_CampaignTypes_CampaignTypeId",
                schema: "Crm",
                table: "Campaigns");

            migrationBuilder.DropTable(
                name: "MarketingListOrganizations",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "MarketingLists",
                schema: "Crm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns",
                schema: "Crm",
                table: "Campaigns");

            migrationBuilder.RenameTable(
                name: "Campaigns",
                schema: "Crm",
                newName: "Campaign",
                newSchema: "Crm");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_CampaignTypeId",
                schema: "Crm",
                table: "Campaign",
                newName: "IX_Campaign_CampaignTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaign",
                schema: "Crm",
                table: "Campaign",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_CampaignTypes_CampaignTypeId",
                schema: "Crm",
                table: "Campaign",
                column: "CampaignTypeId",
                principalSchema: "Crm",
                principalTable: "CampaignTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
