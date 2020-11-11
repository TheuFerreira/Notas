using FluentMigrator;

namespace Notas.Database.Migrations
{
    [Migration(202011111017)]
    public class DropTableTempPostIt : Migration
    {
        public override void Up()
        {
            Delete.Table("tempPostIt");
        }

        public override void Down()
        {
        }
    }
}
