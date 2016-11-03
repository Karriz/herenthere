namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_score_description : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Score", "playerId", "dbo.Player");
            AddColumn("dbo.Score", "spottedPlayerId", c => c.Long(nullable: false));
            AddColumn("dbo.Score", "description", c => c.String());
            AddColumn("dbo.Score", "Player_id", c => c.Long());
            AlterColumn("dbo.Match", "ended", c => c.DateTime());
            CreateIndex("dbo.Score", "spottedPlayerId");
            CreateIndex("dbo.Score", "Player_id");
            AddForeignKey("dbo.Score", "spottedPlayerId", "dbo.Player", "id", cascadeDelete: true);
            AddForeignKey("dbo.Score", "Player_id", "dbo.Player", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Score", "Player_id", "dbo.Player");
            DropForeignKey("dbo.Score", "spottedPlayerId", "dbo.Player");
            DropIndex("dbo.Score", new[] { "Player_id" });
            DropIndex("dbo.Score", new[] { "spottedPlayerId" });
            AlterColumn("dbo.Match", "ended", c => c.DateTime(nullable: false));
            DropColumn("dbo.Score", "Player_id");
            DropColumn("dbo.Score", "description");
            DropColumn("dbo.Score", "spottedPlayerId");
            AddForeignKey("dbo.Score", "playerId", "dbo.Player", "id", cascadeDelete: true);
        }
    }
}
