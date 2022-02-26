namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableandordersentry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Session = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TableEntries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ProductTotal = c.Int(nullable: false),
                        TableID = c.Int(nullable: false),
                        FoodItem_ID = c.Int(),
                        Order_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuItems", t => t.FoodItem_ID)
                .ForeignKey("dbo.Orders", t => t.Order_ID)
                .Index(t => t.FoodItem_ID)
                .Index(t => t.Order_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TableEntries", "Order_ID", "dbo.Orders");
            DropForeignKey("dbo.TableEntries", "FoodItem_ID", "dbo.MenuItems");
            DropIndex("dbo.TableEntries", new[] { "Order_ID" });
            DropIndex("dbo.TableEntries", new[] { "FoodItem_ID" });
            DropTable("dbo.TableEntries");
            DropTable("dbo.Orders");
        }
    }
}
