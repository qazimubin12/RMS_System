namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class grosstotaladded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "GrossTotal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "GrossTotal");
        }
    }
}
