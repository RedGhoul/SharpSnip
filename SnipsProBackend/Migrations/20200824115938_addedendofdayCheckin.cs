using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

namespace Snips.Migrations
{
    public partial class addedendofdayCheckin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndOfDayCheckIns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    WhatWentWell = table.Column<string>(nullable: true),
                    WhatWentBad = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    SearchVector = table.Column<NpgsqlTsVector>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndOfDayCheckIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndOfDayCheckIns_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EndOfDayCheckIns_ApplicationUserId",
                table: "EndOfDayCheckIns",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EndOfDayCheckIns_Created",
                table: "EndOfDayCheckIns",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_EndOfDayCheckIns_LastModified",
                table: "EndOfDayCheckIns",
                column: "LastModified");

            migrationBuilder.CreateIndex(
                name: "IX_EndOfDayCheckIns_SearchVector",
                table: "EndOfDayCheckIns",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndOfDayCheckIns");
        }
    }
}
