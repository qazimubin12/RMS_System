namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somefixes : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.Orders", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Orders", "Session", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Orders", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Orders");
            AlterColumn("dbo.Orders", "Session", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Orders", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Orders", "ID");
        }
    }
}
