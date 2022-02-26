namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablefixesothername : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableEntries", "TableName", c => c.String());
            DropColumn("dbo.TableEntries", "TableID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TableEntries", "TableID", c => c.Int(nullable: false));
            DropColumn("dbo.TableEntries", "TableName");
        }
    }
}
