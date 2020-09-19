using Microsoft.EntityFrameworkCore.Migrations;

namespace Snips.Migrations
{
    public partial class AddedCodeLanguageToNoteRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_ApplicationUserId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoListItems_ToDoLists_ToDoListId",
                table: "ToDoListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoListId",
                table: "ToDoListItems",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CodingLanguageId",
                table: "Notes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CodingLanguageId",
                table: "Notes",
                column: "CodingLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_ApplicationUserId",
                table: "Notes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_CodingLanguages_CodingLanguageId",
                table: "Notes",
                column: "CodingLanguageId",
                principalTable: "CodingLanguages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoListItems_ToDoLists_ToDoListId",
                table: "ToDoListItems",
                column: "ToDoListId",
                principalTable: "ToDoLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_ApplicationUserId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_CodingLanguages_CodingLanguageId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoListItems_ToDoLists_ToDoListId",
                table: "ToDoListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists");

            migrationBuilder.DropIndex(
                name: "IX_Notes_CodingLanguageId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "CodingLanguageId",
                table: "Notes");

            migrationBuilder.AlterColumn<int>(
                name: "ToDoListId",
                table: "ToDoListItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_ApplicationUserId",
                table: "Notes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoListItems_ToDoLists_ToDoListId",
                table: "ToDoListItems",
                column: "ToDoListId",
                principalTable: "ToDoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_AspNetUsers_ApplicationUserId",
                table: "ToDoLists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
