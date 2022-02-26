namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edsa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TableEntries", "FoodItem_ID", "dbo.MenuItems");
            DropIndex("dbo.TableEntries", new[] { "FoodItem_ID" });
            AddColumn("dbo.TableEntries", "FoodItemID", c => c.Int(nullable: false));
            DropColumn("dbo.TableEntries", "FoodItem_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TableEntries", "FoodItem_ID", c => c.Int());
            DropColumn("dbo.TableEntries", "FoodItemID");
            CreateIndex("dbo.TableEntries", "FoodItem_ID");
            AddForeignKey("dbo.TableEntries", "FoodItem_ID", "dbo.MenuItems", "ID");
        }
    }
}
