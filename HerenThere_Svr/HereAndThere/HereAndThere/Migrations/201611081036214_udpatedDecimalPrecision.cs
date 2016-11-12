namespace HereAndThere.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class udpatedDecimalPrecision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Boundary", "latitude", c => c.Decimal(nullable: false, precision: 16, scale: 10));
            AlterColumn("dbo.Boundary", "longitude", c => c.Decimal(nullable: false, precision: 16, scale: 10));
            AlterColumn("dbo.Location", "latitude", c => c.Decimal(nullable: false, precision: 16, scale: 10));
            AlterColumn("dbo.Location", "longitude", c => c.Decimal(nullable: false, precision: 16, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Location", "longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Location", "latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Boundary", "longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Boundary", "latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
