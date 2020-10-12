using FluentMigrator;

namespace Notas.Database.Migrations
{
    [Migration(202010062213)]
    public class AddColumnPosition : Migration
    {
        public override void Up()
        {
            Alter.Table("postit")
                .AddColumn("position").AsInt32().WithDefaultValue(1).NotNullable();
        }

        public override void Down()
        {
            Delete.Column("position").FromTable("postit");
        }
    }
}
