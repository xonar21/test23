using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddOrganizationComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: new DateTime(2021, 3, 2, 10, 58, 32, 549, DateTimeKind.Utc).AddTicks(3535),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 2, 22, 14, 16, 18, 573, DateTimeKind.Utc).AddTicks(6567));

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                schema: "Crm",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "Crm",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OrganizationId",
                schema: "Crm",
                table: "Comments",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Organizations_OrganizationId",
                schema: "Crm",
                table: "Comments",
                column: "OrganizationId",
                principalSchema: "Crm",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Organizations_OrganizationId",
                schema: "Crm",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_OrganizationId",
                schema: "Crm",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "Crm",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StageChangeDate",
                schema: "Crm",
                table: "Leads",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 22, 14, 16, 18, 573, DateTimeKind.Utc).AddTicks(6567),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 3, 2, 10, 58, 32, 549, DateTimeKind.Utc).AddTicks(3535));

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                schema: "Crm",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
