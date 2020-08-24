using Microsoft.EntityFrameworkCore.Migrations;

namespace Snips.Migrations
{
    public partial class addedendofdayCheckin_TSVectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"CREATE TRIGGER endofdaycheckin_search_vector_update BEFORE INSERT OR UPDATE
              ON ""EndOfDayCheckIns"" FOR EACH ROW EXECUTE PROCEDURE
              tsvector_update_trigger(""SearchVector"", 'pg_catalog.english',
                ""Comments"",""WhatWentWell"",""WhatWentBad"");");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER endofdaycheckin_search_vector_update on ""EndOfDayCheckIns""");
        }
    }
}
