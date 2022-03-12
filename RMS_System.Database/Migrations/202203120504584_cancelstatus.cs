namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cancelstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableEntries", "CancelledStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TableEntries", "CancelledStatus");
        }
    }
}
