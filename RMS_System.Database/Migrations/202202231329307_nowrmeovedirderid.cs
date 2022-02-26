namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nowrmeovedirderid : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TableEntries", "OrderID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TableEntries", "OrderID", c => c.Int(nullable: false));
        }
    }
}
