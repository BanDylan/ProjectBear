namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileStrikesAndBans : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "Strikes", c => c.Int(nullable: false));
            AddColumn("dbo.Profiles", "Banned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "Banned");
            DropColumn("dbo.Profiles", "Strikes");
        }
    }
}
