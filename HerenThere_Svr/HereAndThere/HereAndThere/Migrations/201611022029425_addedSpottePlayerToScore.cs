namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedSpottePlayerToScore : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Score", "playerId", "dbo.Player");
            DropIndex("dbo.Score", new[] { "playerId" });
            DropIndex("dbo.Score", new[] { "Player_id" });
            DropColumn("dbo.Score", "playerId");
            RenameColumn(table: "dbo.Score", name: "Player_id", newName: "playerId");
            AlterColumn("dbo.Score", "playerId", c => c.Long(nullable: false));
            CreateIndex("dbo.Score", "playerId");
            AddForeignKey("dbo.Score", "playerId", "dbo.Player", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Score", "playerId", "dbo.Player");
            DropIndex("dbo.Score", new[] { "playerId" });
            AlterColumn("dbo.Score", "playerId", c => c.Long());
            RenameColumn(table: "dbo.Score", name: "playerId", newName: "Player_id");
            AddColumn("dbo.Score", "playerId", c => c.Long(nullable: false));
            CreateIndex("dbo.Score", "Player_id");
            CreateIndex("dbo.Score", "playerId");
            AddForeignKey("dbo.Score", "playerId", "dbo.Player", "id", cascadeDelete: true);
        }
    }
}
