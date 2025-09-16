using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetSamples.Database.Migrations
{
    /// <inheritdoc />
    public partial class DesciptionPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionPictureUrl",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionPictureUrl",
                table: "Articles");
        }
    }
}
