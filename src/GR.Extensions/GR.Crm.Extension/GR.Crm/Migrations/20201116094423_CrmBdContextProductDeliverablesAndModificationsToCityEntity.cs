using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmBdContextProductDeliverablesAndModificationsToCityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechnologStack",
                schema: "Crm",
                table: "ProductVariations");

            migrationBuilder.AddColumn<string>(
                name: "TechnologyStack",
                schema: "Crm",
                table: "ProductVariations",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CrmCountryId",
                schema: "Crm",
                table: "Cities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProductDeliverables",
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
                    ProductVariationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDeliverables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDeliverables_ProductVariations_ProductVariationId",
                        column: x => x.ProductVariationId,
                        principalSchema: "Crm",
                        principalTable: "ProductVariations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CrmCountryId",
                schema: "Crm",
                table: "Cities",
                column: "CrmCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDeliverables_ProductVariationId",
                schema: "Crm",
                table: "ProductDeliverables",
                column: "ProductVariationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CrmCountryId",
                schema: "Crm",
                table: "Cities",
                column: "CrmCountryId",
                principalSchema: "Crm",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CrmCountryId",
                schema: "Crm",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "ProductDeliverables",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CrmCountryId",
                schema: "Crm",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "TechnologyStack",
                schema: "Crm",
                table: "ProductVariations");

            migrationBuilder.DropColumn(
                name: "CrmCountryId",
                schema: "Crm",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "TechnologStack",
                schema: "Crm",
                table: "ProductVariations",
                nullable: true);
        }
    }
}
