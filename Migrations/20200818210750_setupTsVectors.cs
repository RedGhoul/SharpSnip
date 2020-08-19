using Microsoft.EntityFrameworkCore.Migrations;

namespace Snips.Migrations
{
    public partial class setupTsVectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"CREATE TRIGGER note_search_vector_update BEFORE INSERT OR UPDATE
              ON ""Notes"" FOR EACH ROW EXECUTE PROCEDURE
              tsvector_update_trigger(""SearchVector"", 'pg_catalog.english', ""Name"", ""Content"",""CodeContent"");");


            migrationBuilder.Sql(
            @"CREATE TRIGGER todolist_search_vector_update BEFORE INSERT OR UPDATE
              ON ""ToDoLists"" FOR EACH ROW EXECUTE PROCEDURE
              tsvector_update_trigger(""SearchVector"", 'pg_catalog.english', ""Name"", ""Description"");");


            migrationBuilder.Sql(
            @"CREATE TRIGGER todolistitem_search_vector_update BEFORE INSERT OR UPDATE
              ON ""ToDoListItems"" FOR EACH ROW EXECUTE PROCEDURE
              tsvector_update_trigger(""SearchVector"", 'pg_catalog.english', ""Content"");");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER note_search_vector_update on ""Notes""");
            migrationBuilder.Sql(@"DROP TRIGGER todolist_search_vector_update on ""ToDoLists""");
            migrationBuilder.Sql(@"DROP TRIGGER todolistitem_search_vector_update on ""ToDoListItems""");

        }
    }
}
