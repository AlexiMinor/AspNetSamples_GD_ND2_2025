using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetSamples.Database.Migrations
{
    /// <inheritdoc />
    public partial class RssLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RssLink",
                table: "Sources",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RssLink",
                table: "Sources");
        }
    }
}
