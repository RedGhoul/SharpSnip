using Microsoft.EntityFrameworkCore.Migrations;

namespace Snips.Migrations
{
    public partial class AddedCodeLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeLanguage",
                table: "Notes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeLanguage",
                table: "Notes");
        }
    }
}
