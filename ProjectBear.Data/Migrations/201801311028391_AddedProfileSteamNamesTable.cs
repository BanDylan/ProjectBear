namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProfileSteamNamesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileSteamNames",
                c => new
                    {
                        ProfileSteamNameId = c.Guid(nullable: false, identity: true),
                        ProfileId = c.Guid(nullable: false),
                        SteamName = c.String(nullable: false),
                        FirstUsedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProfileSteamNameId)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProfileSteamNames", "ProfileId", "dbo.Profiles");
            DropIndex("dbo.ProfileSteamNames", new[] { "ProfileId" });
            DropTable("dbo.ProfileSteamNames");
        }
    }
}
