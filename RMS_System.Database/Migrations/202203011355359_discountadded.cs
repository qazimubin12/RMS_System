namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discountadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DiscountPercentage", c => c.Double(nullable: false));
            AddColumn("dbo.Orders", "Discount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Discount");
            DropColumn("dbo.Orders", "DiscountPercentage");
        }
    }
}
