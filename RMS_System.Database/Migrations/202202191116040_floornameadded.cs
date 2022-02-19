namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class floornameadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tables", "FloorName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tables", "FloorName");
        }
    }
}
