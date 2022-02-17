namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somefixed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRoleMappings", "RoleID_ID", "dbo.Roles");
            DropForeignKey("dbo.UserRoleMappings", "UserID_ID", "dbo.Users");
            DropIndex("dbo.UserRoleMappings", new[] { "RoleID_ID" });
            DropIndex("dbo.UserRoleMappings", new[] { "UserID_ID" });
            AddColumn("dbo.UserRoleMappings", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.UserRoleMappings", "RoleID", c => c.Int(nullable: false));
            DropColumn("dbo.UserRoleMappings", "RoleID_ID");
            DropColumn("dbo.UserRoleMappings", "UserID_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRoleMappings", "UserID_ID", c => c.Int());
            AddColumn("dbo.UserRoleMappings", "RoleID_ID", c => c.Int());
            DropColumn("dbo.UserRoleMappings", "RoleID");
            DropColumn("dbo.UserRoleMappings", "UserID");
            CreateIndex("dbo.UserRoleMappings", "UserID_ID");
            CreateIndex("dbo.UserRoleMappings", "RoleID_ID");
            AddForeignKey("dbo.UserRoleMappings", "UserID_ID", "dbo.Users", "ID");
            AddForeignKey("dbo.UserRoleMappings", "RoleID_ID", "dbo.Roles", "ID");
        }
    }
}
