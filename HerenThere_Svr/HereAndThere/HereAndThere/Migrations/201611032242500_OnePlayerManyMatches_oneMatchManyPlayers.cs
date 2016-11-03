namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnePlayerManyMatches_oneMatchManyPlayers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerMatch",
                c => new
                    {
                        playerId = c.Long(nullable: false),
                        matchId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.playerId, t.matchId })
                .ForeignKey("dbo.Player", t => t.playerId, cascadeDelete: true)
                .ForeignKey("dbo.Match", t => t.matchId, cascadeDelete: true)
                .Index(t => t.playerId)
                .Index(t => t.matchId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerMatch", "matchId", "dbo.Match");
            DropForeignKey("dbo.PlayerMatch", "playerId", "dbo.Player");
            DropIndex("dbo.PlayerMatch", new[] { "matchId" });
            DropIndex("dbo.PlayerMatch", new[] { "playerId" });
            DropTable("dbo.PlayerMatch");
        }
    }
}
