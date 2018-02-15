namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedStrikeCounterAndAddedProfileStrikeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileStrikes",
                c => new
                    {
                        ProfileStrikeId = c.Guid(nullable: false, identity: true),
                        ProfileId = c.Guid(nullable: false),
                        Reason = c.String(nullable: false),
                        DateIssued = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileStrikeId)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId);
            
            DropColumn("dbo.Profiles", "Strikes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profiles", "Strikes", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProfileStrikes", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.ProfileStrikes", new[] { "ProfileId" });
            DropTable("dbo.ProfileStrikes");
        }
    }
}
