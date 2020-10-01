using FluentMigrator;

namespace Notas.Database.Migrations
{
    [Migration(202010011118)]
    public class AddColumnColor : Migration
    {
        public override void Up()
        {
            Alter.Table("postit").
                AddColumn("color").AsString(7).NotNullable().WithDefaultValue("#1B1B1B");
        }

        public override void Down()
        {
            Delete.Column("color").FromTable("postit");
        }
    }
}
