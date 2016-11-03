namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelRefactoring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Match", "startTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Match", "endTime", c => c.DateTime());
            DropColumn("dbo.Match", "started");
            DropColumn("dbo.Match", "ended");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Match", "ended", c => c.DateTime());
            AddColumn("dbo.Match", "started", c => c.DateTime(nullable: false));
            DropColumn("dbo.Match", "endTime");
            DropColumn("dbo.Match", "startTime");
        }
    }
}
