using Microsoft.EntityFrameworkCore.Migrations;

namespace Snips.Migrations
{
    public partial class AddedContentAndBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeContent",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCode",
                table: "Notes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeContent",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "HasCode",
                table: "Notes");
        }
    }
}
