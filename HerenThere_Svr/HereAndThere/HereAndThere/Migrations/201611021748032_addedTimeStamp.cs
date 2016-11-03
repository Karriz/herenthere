namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTimeStamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boundary", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Boundary", "modifiedTimeStamp", c => c.DateTime());
            AddColumn("dbo.Location", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Location", "modifiedTimeStamp", c => c.DateTime());
            AddColumn("dbo.Movement", "isMoving", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movement", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Movement", "modifiedTimeStamp", c => c.DateTime());
            AddColumn("dbo.Match", "started", c => c.DateTime(nullable: false));
            AddColumn("dbo.Match", "ended", c => c.DateTime(nullable: false));
            AddColumn("dbo.Match", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Match", "modifiedTimeStamp", c => c.DateTime());
            AddColumn("dbo.MatchType", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.MatchType", "modifiedTimeStamp", c => c.DateTime());
            AddColumn("dbo.Score", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Score", "modifiedTimeStamp", c => c.DateTime());
            AddColumn("dbo.Player", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Player", "modifiedTimeStamp", c => c.DateTime());
            AddColumn("dbo.PlayerType", "timeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.PlayerType", "modifiedTimeStamp", c => c.DateTime());
            AlterColumn("dbo.Boundary", "modifiedBy", c => c.String(maxLength: 32));
            AlterColumn("dbo.Location", "modifiedBy", c => c.String(maxLength: 32));
            AlterColumn("dbo.Movement", "modifiedBy", c => c.String(maxLength: 32));
            AlterColumn("dbo.Match", "modifiedBy", c => c.String(maxLength: 32));
            AlterColumn("dbo.MatchType", "modifiedBy", c => c.String(maxLength: 32));
            AlterColumn("dbo.Score", "modifiedBy", c => c.String(maxLength: 32));
            AlterColumn("dbo.Player", "modifiedBy", c => c.String(maxLength: 32));
            AlterColumn("dbo.PlayerType", "modifiedBy", c => c.String(maxLength: 32));
            DropColumn("dbo.Boundary", "created");
            DropColumn("dbo.Boundary", "modified");
            DropColumn("dbo.Location", "created");
            DropColumn("dbo.Location", "modified");
            DropColumn("dbo.Movement", "created");
            DropColumn("dbo.Movement", "modified");
            DropColumn("dbo.Match", "created");
            DropColumn("dbo.Match", "modified");
            DropColumn("dbo.MatchType", "created");
            DropColumn("dbo.MatchType", "modified");
            DropColumn("dbo.Score", "created");
            DropColumn("dbo.Score", "modified");
            DropColumn("dbo.Player", "created");
            DropColumn("dbo.Player", "modified");
            DropColumn("dbo.PlayerType", "created");
            DropColumn("dbo.PlayerType", "modified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PlayerType", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.PlayerType", "created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Player", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Player", "created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Score", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Score", "created", c => c.DateTime(nullable: false));
            AddColumn("dbo.MatchType", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.MatchType", "created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Match", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Match", "created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Movement", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Movement", "created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Location", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Location", "created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Boundary", "modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Boundary", "created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PlayerType", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Player", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Score", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.MatchType", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Match", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Movement", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Location", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Boundary", "modifiedBy", c => c.String(nullable: false, maxLength: 32));
            DropColumn("dbo.PlayerType", "modifiedTimeStamp");
            DropColumn("dbo.PlayerType", "timeStamp");
            DropColumn("dbo.Player", "modifiedTimeStamp");
            DropColumn("dbo.Player", "timeStamp");
            DropColumn("dbo.Score", "modifiedTimeStamp");
            DropColumn("dbo.Score", "timeStamp");
            DropColumn("dbo.MatchType", "modifiedTimeStamp");
            DropColumn("dbo.MatchType", "timeStamp");
            DropColumn("dbo.Match", "modifiedTimeStamp");
            DropColumn("dbo.Match", "timeStamp");
            DropColumn("dbo.Match", "ended");
            DropColumn("dbo.Match", "started");
            DropColumn("dbo.Movement", "modifiedTimeStamp");
            DropColumn("dbo.Movement", "timeStamp");
            DropColumn("dbo.Movement", "isMoving");
            DropColumn("dbo.Location", "modifiedTimeStamp");
            DropColumn("dbo.Location", "timeStamp");
            DropColumn("dbo.Boundary", "modifiedTimeStamp");
            DropColumn("dbo.Boundary", "timeStamp");
        }
    }
}
