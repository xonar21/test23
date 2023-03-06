using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddNoGoState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NoGoStateId",
                schema: "Crm",
                table: "Leads",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NoGoStates",
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
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoGoStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leads_NoGoStateId",
                schema: "Crm",
                table: "Leads",
                column: "NoGoStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_NoGoStates_NoGoStateId",
                schema: "Crm",
                table: "Leads",
                column: "NoGoStateId",
                principalSchema: "Crm",
                principalTable: "NoGoStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_NoGoStates_NoGoStateId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropTable(
                name: "NoGoStates",
                schema: "Crm");

            migrationBuilder.DropIndex(
                name: "IX_Leads_NoGoStateId",
                schema: "Crm",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "NoGoStateId",
                schema: "Crm",
                table: "Leads");
        }
    }
}
