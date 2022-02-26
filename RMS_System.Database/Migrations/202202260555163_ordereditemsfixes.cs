namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ordereditemsfixes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ItemsServed", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OrderedItems", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderedItems");
            DropColumn("dbo.Orders", "ItemsServed");
        }
    }
}
