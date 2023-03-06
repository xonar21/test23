using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_UpdateProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commission",
                schema: "Crm",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Vat",
                schema: "Crm",
                table: "Products",
                newName: "Warranty");

            migrationBuilder.RenameColumn(
                name: "Sku",
                schema: "Crm",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "Crm",
                table: "Products",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "BankAccount",
                schema: "Crm",
                table: "Products",
                newName: "ProductCode");

            migrationBuilder.AddColumn<bool>(
                name: "CanBeSold",
                schema: "Crm",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                schema: "Crm",
                table: "Products",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                schema: "Crm",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EAN",
                schema: "Crm",
                table: "Products",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductManufactoriesId",
                schema: "Crm",
                table: "Products",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductTypeId",
                schema: "Crm",
                table: "Products",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "Crm",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CurrencyCode",
                schema: "Crm",
                table: "Products",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductManufactoriesId",
                schema: "Crm",
                table: "Products",
                column: "ProductManufactoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                schema: "Crm",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "Crm",
                table: "Products",
                column: "CategoryId",
                principalSchema: "Crm",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Currencies_CurrencyCode",
                schema: "Crm",
                table: "Products",
                column: "CurrencyCode",
                principalSchema: "Crm",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductManufactories_ProductManufactoriesId",
                schema: "Crm",
                table: "Products",
                column: "ProductManufactoriesId",
                principalSchema: "Crm",
                principalTable: "ProductManufactories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "Products",
                column: "ProductTypeId",
                principalSchema: "Crm",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Currencies_CurrencyCode",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductManufactories_ProductManufactoriesId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CurrencyCode",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductManufactoriesId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductTypeId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CanBeSold",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EAN",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductManufactoriesId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                schema: "Crm",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Warranty",
                schema: "Crm",
                table: "Products",
                newName: "Vat");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                schema: "Crm",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                schema: "Crm",
                table: "Products",
                newName: "BankAccount");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "Crm",
                table: "Products",
                newName: "Sku");

            migrationBuilder.AddColumn<decimal>(
                name: "Commission",
                schema: "Crm",
                table: "Products",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
