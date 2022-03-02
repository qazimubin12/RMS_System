namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taxesforreceipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "SGST", c => c.Double(nullable: false));
            AddColumn("dbo.Bills", "CGST", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "CGST");
            DropColumn("dbo.Bills", "SGST");
        }
    }
}
