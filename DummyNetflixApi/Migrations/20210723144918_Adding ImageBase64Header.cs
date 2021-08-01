using Microsoft.EntityFrameworkCore.Migrations;

namespace DummyNetflixApi.Migrations
{
    public partial class AddingImageBase64Header : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageHeaderBase64",
                table: "Movies",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageHeaderBase64",
                table: "Movies");
        }
    }
}
