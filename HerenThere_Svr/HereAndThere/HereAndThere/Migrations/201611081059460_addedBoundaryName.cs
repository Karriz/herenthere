namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedBoundaryName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boundary", "name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boundary", "name");
        }
    }
}
