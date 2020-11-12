using FluentMigrator;

namespace Notas.Database.Migrations
{
    [Migration(202011121256)]
    public class AddColumnFontColor : Migration
    {
        public override void Up()
        {
            Alter.Table("postit").AddColumn("fontColor").AsString(7).Nullable();
        }

        public override void Down()
        {
        }
    }
}
