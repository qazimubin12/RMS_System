namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userfixing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UserRole_ID", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "UserRole_ID" });
            AddColumn("dbo.Users", "Role", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "UserRole_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserRole_ID", c => c.Int());
            DropColumn("dbo.Users", "Role");
            CreateIndex("dbo.Users", "UserRole_ID");
            AddForeignKey("dbo.Users", "UserRole_ID", "dbo.Roles", "ID");
        }
    }
}
