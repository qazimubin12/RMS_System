namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TableName = c.String(),
                        Seats = c.Int(nullable: false),
                        OrderItems = c.Int(),
                        ItemsServed = c.Int(),
                        ServedBy = c.String(),
                        TableStatus = c.String(),
                        SessionStatus = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tables");
        }
    }
}
