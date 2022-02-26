namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grandtotaladded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "GrandTotal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "GrandTotal");
        }
    }
}
