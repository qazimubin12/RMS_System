namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spanedtime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableEntries", "OrderedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TableEntries", "OrderedTime");
        }
    }
}
