namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedorders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TableEntries", "Order_ID", "dbo.Orders");
            DropIndex("dbo.TableEntries", new[] { "Order_ID" });
            DropColumn("dbo.TableEntries", "Order_ID");
            DropTable("dbo.Orders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Session = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.TableEntries", "Order_ID", c => c.Int());
            CreateIndex("dbo.TableEntries", "Order_ID");
            AddForeignKey("dbo.TableEntries", "Order_ID", "dbo.Orders", "ID");
        }
    }
}
