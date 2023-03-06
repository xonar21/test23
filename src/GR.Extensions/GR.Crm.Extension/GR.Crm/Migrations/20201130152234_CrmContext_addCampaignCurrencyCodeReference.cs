using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmContext_addCampaignCurrencyCodeReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "Crm",
                table: "Campaigns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CurrencyCode",
                schema: "Crm",
                table: "Campaigns",
                column: "CurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Currencies_CurrencyCode",
                schema: "Crm",
                table: "Campaigns",
                column: "CurrencyCode",
                principalSchema: "Crm",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Currencies_CurrencyCode",
                schema: "Crm",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_CurrencyCode",
                schema: "Crm",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "Crm",
                table: "Campaigns");
        }
    }
}
