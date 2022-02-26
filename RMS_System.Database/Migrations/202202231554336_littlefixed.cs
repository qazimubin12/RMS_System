namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class littlefixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableEntries", "FoodItem", c => c.String());
            DropColumn("dbo.TableEntries", "FoodItemID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TableEntries", "FoodItemID", c => c.Int(nullable: false));
            DropColumn("dbo.TableEntries", "FoodItem");
        }
    }
}
