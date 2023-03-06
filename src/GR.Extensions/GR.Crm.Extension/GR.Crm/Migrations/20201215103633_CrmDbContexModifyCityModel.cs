using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContexModifyCityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                schema: "Crm",
                table: "Cities");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                schema: "Crm",
                table: "Cities",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                schema: "Crm",
                table: "Cities",
                column: "RegionId",
                principalSchema: "Crm",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                schema: "Crm",
                table: "Cities");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegionId",
                schema: "Crm",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                schema: "Crm",
                table: "Cities",
                column: "RegionId",
                principalSchema: "Crm",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
