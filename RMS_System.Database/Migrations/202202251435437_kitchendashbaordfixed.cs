namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kitchendashbaordfixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TableName", c => c.String());
            AddColumn("dbo.Orders", "WaiterName", c => c.String());
            DropColumn("dbo.Orders", "TableID");
            DropColumn("dbo.Orders", "WaiterID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "WaiterID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "TableID", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "WaiterName");
            DropColumn("dbo.Orders", "TableName");
        }
    }
}
