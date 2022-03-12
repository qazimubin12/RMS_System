namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class separatecancelled : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TableEntries", "CancelledStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TableEntries", "CancelledStatus", c => c.Boolean(nullable: false));
        }
    }
}
