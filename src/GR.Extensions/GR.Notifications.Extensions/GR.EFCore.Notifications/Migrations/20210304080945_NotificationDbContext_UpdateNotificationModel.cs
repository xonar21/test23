using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Notifications.Migrations
{
    public partial class NotificationDbContext_UpdateNotificationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                schema: "Notifications",
                table: "Notifications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                schema: "Notifications",
                table: "Notifications");
        }
    }
}
