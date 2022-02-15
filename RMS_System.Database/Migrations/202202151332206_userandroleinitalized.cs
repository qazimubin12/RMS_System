namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userandroleinitalized : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Roles = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        ImageURL = c.String(),
                        UserRole_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Roles", t => t.UserRole_ID)
                .Index(t => t.UserRole_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserRole_ID", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "UserRole_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
        }
    }
}
