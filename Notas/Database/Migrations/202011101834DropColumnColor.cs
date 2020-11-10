using FluentMigrator;

namespace Notas.Database.Migrations
{
    [Migration(202011101834)]
    public class DropColumnColor : Migration
    {
        public override void Up()
        {
            Execute.Sql("" +
                "CREATE TABLE tempPostit( " +
                "id INTEGER NOT NULL primary key not null, " +
                "content STRING NOT NULL, " +
                "position INTEGER NOT NULL " +
                "); " +

                "INSERT INTO tempPostit SELECT id, content, position FROM postit; " +

                "DROP TABLE postit; " +

                "CREATE TABLE postit( " +
                "id INTEGER NOT NULL primary key not null, " +
                "content STRING NOT NULL, " +
                "position INTEGER NOT NULL " +
                "); " +

                "INSERT INTO postit SELECT id, content, position FROM tempPostit;");

            Alter.Table("postit").AddColumn("color").AsString(7).Nullable();
        }

        public override void Down()
        {
            Delete.Column("color").FromTable("postit");

            Alter.Table("postit").
                AddColumn("color").AsString(7).NotNullable().WithDefaultValue("#1B1B1B");
        }
    }
}
