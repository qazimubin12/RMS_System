namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sessionremoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "Session");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Session", c => c.Int(nullable: false));
        }
    }
}
