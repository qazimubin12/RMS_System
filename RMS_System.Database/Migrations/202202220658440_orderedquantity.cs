namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderedquantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "OrderedQuantity", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItems", "OrderedQuantity");
        }
    }
}
