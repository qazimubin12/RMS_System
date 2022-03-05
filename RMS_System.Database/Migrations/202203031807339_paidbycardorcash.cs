namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paidbycardorcash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PaidBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PaidBy");
        }
    }
}
