using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbConext_AddCommntAssigndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentAssignedUsers",
                schema: "Crm",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    CommentId = table.Column<Guid>(nullable: false),
                    UserEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentAssignedUsers", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentAssignedUsers_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "Crm",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentAssignedUsers",
                schema: "Crm");
        }
    }
}
