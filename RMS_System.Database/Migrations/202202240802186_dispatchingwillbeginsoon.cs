namespace RMS_System.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dispatchingwillbeginsoon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TableEntries", "FoodDispatchedStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TableEntries", "FoodDispatchedStatus");
        }
    }
}
