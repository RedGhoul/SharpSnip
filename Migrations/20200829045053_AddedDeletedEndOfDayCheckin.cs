using Microsoft.EntityFrameworkCore.Migrations;

namespace Snips.Migrations
{
    public partial class AddedDeletedEndOfDayCheckin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "EndOfDayCheckIns",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "EndOfDayCheckIns");
        }
    }
}
