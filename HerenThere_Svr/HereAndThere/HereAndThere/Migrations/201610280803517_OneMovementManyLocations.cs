namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OneMovementManyLocations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movement", "locationId", "dbo.Location");
            DropIndex("dbo.Movement", new[] { "locationId" });
            AddColumn("dbo.Location", "movementId", c => c.Long(nullable: false));
            AddColumn("dbo.Score", "point", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Location", "movementId");
            AddForeignKey("dbo.Location", "movementId", "dbo.Movement", "id", cascadeDelete: true);
            DropColumn("dbo.Score", "polong");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Score", "polong", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.Location", "movementId", "dbo.Movement");
            DropIndex("dbo.Location", new[] { "movementId" });
            DropColumn("dbo.Score", "point");
            DropColumn("dbo.Location", "movementId");
            CreateIndex("dbo.Movement", "locationId");
            AddForeignKey("dbo.Movement", "locationId", "dbo.Location", "id", cascadeDelete: true);
        }
    }
}
