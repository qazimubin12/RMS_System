namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableforcancelledadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CancelledOrders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        CancelledDate = c.DateTime(nullable: false),
                        WaiterName = c.String(),
                        TableName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CancelledOrders");
        }
    }
}
