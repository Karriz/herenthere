namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boundary",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        matchId = c.Long(nullable: false),
                        latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Match", t => t.matchId, cascadeDelete: true)
                .Index(t => t.matchId);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isActual = c.Boolean(nullable: false),
                        isVisible = c.Boolean(nullable: false),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        matchTypeId = c.Long(nullable: false),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.MatchType", t => t.matchTypeId, cascadeDelete: true)
                .Index(t => t.matchTypeId);
            
            CreateTable(
                "dbo.MatchType",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32),
                        description = c.String(),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Movement",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        playerId = c.Long(nullable: false),
                        matchId = c.Long(nullable: false),
                        locationId = c.Long(nullable: false),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Location", t => t.locationId, cascadeDelete: true)
                .ForeignKey("dbo.Match", t => t.matchId, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.playerId, cascadeDelete: true)
                .Index(t => t.playerId)
                .Index(t => t.matchId)
                .Index(t => t.locationId);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32),
                        playerTypeId = c.Long(nullable: false),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PlayerType", t => t.playerTypeId, cascadeDelete: true)
                .Index(t => t.name, unique: true)
                .Index(t => t.playerTypeId);
            
            CreateTable(
                "dbo.PlayerType",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32),
                        description = c.String(),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Score",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        matchId = c.Long(nullable: false),
                        playerId = c.Long(nullable: false),
                        polong = c.Decimal(nullable: false, precision: 18, scale: 2),
                        created = c.DateTime(nullable: false),
                        createdBy = c.String(nullable: false, maxLength: 32),
                        modified = c.DateTime(nullable: false),
                        modifiedBy = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Match", t => t.matchId, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.playerId, cascadeDelete: true)
                .Index(t => t.matchId)
                .Index(t => t.playerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Score", "playerId", "dbo.Player");
            DropForeignKey("dbo.Score", "matchId", "dbo.Match");
            DropForeignKey("dbo.Player", "playerTypeId", "dbo.PlayerType");
            DropForeignKey("dbo.Movement", "playerId", "dbo.Player");
            DropForeignKey("dbo.Movement", "matchId", "dbo.Match");
            DropForeignKey("dbo.Movement", "locationId", "dbo.Location");
            DropForeignKey("dbo.Match", "matchTypeId", "dbo.MatchType");
            DropForeignKey("dbo.Boundary", "matchId", "dbo.Match");
            DropIndex("dbo.Score", new[] { "playerId" });
            DropIndex("dbo.Score", new[] { "matchId" });
            DropIndex("dbo.Player", new[] { "playerTypeId" });
            DropIndex("dbo.Player", new[] { "name" });
            DropIndex("dbo.Movement", new[] { "locationId" });
            DropIndex("dbo.Movement", new[] { "matchId" });
            DropIndex("dbo.Movement", new[] { "playerId" });
            DropIndex("dbo.Match", new[] { "matchTypeId" });
            DropIndex("dbo.Boundary", new[] { "matchId" });
            DropTable("dbo.Score");
            DropTable("dbo.PlayerType");
            DropTable("dbo.Player");
            DropTable("dbo.Movement");
            DropTable("dbo.MatchType");
            DropTable("dbo.Match");
            DropTable("dbo.Location");
            DropTable("dbo.Boundary");
        }
    }
}
