using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckZone.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscordWebhookUrlToConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "discord_webhook_url",
                table: "SystemConfigurations",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discord_webhook_url",
                table: "SystemConfigurations");
        }
    }
}
