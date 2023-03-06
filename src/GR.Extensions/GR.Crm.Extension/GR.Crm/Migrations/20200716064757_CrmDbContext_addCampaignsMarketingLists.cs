using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_addCampaignsMarketingLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignsMarketingLists",
                schema: "Crm",
                columns: table => new
                {
                    CampaignId = table.Column<Guid>(nullable: false),
                    MarketingListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignsMarketingLists", x => new { x.CampaignId, x.MarketingListId });
                    table.ForeignKey(
                        name: "FK_CampaignsMarketingLists_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "Crm",
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignsMarketingLists_MarketingLists_MarketingListId",
                        column: x => x.MarketingListId,
                        principalSchema: "Crm",
                        principalTable: "MarketingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignsMarketingLists_MarketingListId",
                schema: "Crm",
                table: "CampaignsMarketingLists",
                column: "MarketingListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignsMarketingLists",
                schema: "Crm");
        }
    }
}
