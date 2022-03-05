namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billingdoneornot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableEntries", "BillingDone", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TableEntries", "BillingDone");
        }
    }
}
