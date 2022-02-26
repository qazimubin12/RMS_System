namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ordetable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Session = c.Int(nullable: false, identity: true),
                        TableID = c.Int(nullable: false),
                        WaiterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.TableEntries", "OrderID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TableEntries", "OrderID");
            DropTable("dbo.Orders");
        }
    }
}
