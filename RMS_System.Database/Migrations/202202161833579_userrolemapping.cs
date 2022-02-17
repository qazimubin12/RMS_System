namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userrolemapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRoleMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleID_ID = c.Int(),
                        UserID_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Roles", t => t.RoleID_ID)
                .ForeignKey("dbo.Users", t => t.UserID_ID)
                .Index(t => t.RoleID_ID)
                .Index(t => t.UserID_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoleMappings", "UserID_ID", "dbo.Users");
            DropForeignKey("dbo.UserRoleMappings", "RoleID_ID", "dbo.Roles");
            DropIndex("dbo.UserRoleMappings", new[] { "UserID_ID" });
            DropIndex("dbo.UserRoleMappings", new[] { "RoleID_ID" });
            DropTable("dbo.UserRoleMappings");
        }
    }
}
