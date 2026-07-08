using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckZone.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesForClientFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "verifier_name",
                table: "ScamReports",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "verifier_zalo",
                table: "ScamReports",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "LegitProfiles",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "facebook",
                table: "LegitProfiles",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "website",
                table: "LegitProfiles",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "thumbnail_url",
                table: "BlogArticles",
                type: "varchar(1024)",
                maxLength: 1024,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "verifier_name",
                table: "ScamReports");

            migrationBuilder.DropColumn(
                name: "verifier_zalo",
                table: "ScamReports");

            migrationBuilder.DropColumn(
                name: "address",
                table: "LegitProfiles");

            migrationBuilder.DropColumn(
                name: "facebook",
                table: "LegitProfiles");

            migrationBuilder.DropColumn(
                name: "website",
                table: "LegitProfiles");

            migrationBuilder.DropColumn(
                name: "thumbnail_url",
                table: "BlogArticles");
        }
    }
}
