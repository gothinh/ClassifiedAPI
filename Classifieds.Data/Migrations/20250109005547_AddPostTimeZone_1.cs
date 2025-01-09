using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Classifieds.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostTimeZone_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Posts");
        }
    }
}
