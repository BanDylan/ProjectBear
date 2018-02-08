namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwappedSteamNameToNonSteamNameRemovedTwitchNameFromBookings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerInTimeSlots", "NonSteamName", c => c.String());
            AddColumn("dbo.ReserveInTimeSlots", "NonSteamName", c => c.String());
            DropColumn("dbo.PlayerInTimeSlots", "SteamName");
            DropColumn("dbo.PlayerInTimeSlots", "TwitchName");
            DropColumn("dbo.ReserveInTimeSlots", "SteamName");
            DropColumn("dbo.ReserveInTimeSlots", "TwitchName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReserveInTimeSlots", "TwitchName", c => c.String(nullable: false));
            AddColumn("dbo.ReserveInTimeSlots", "SteamName", c => c.String(nullable: false));
            AddColumn("dbo.PlayerInTimeSlots", "TwitchName", c => c.String(nullable: false));
            AddColumn("dbo.PlayerInTimeSlots", "SteamName", c => c.String(nullable: false));
            DropColumn("dbo.ReserveInTimeSlots", "NonSteamName");
            DropColumn("dbo.PlayerInTimeSlots", "NonSteamName");
        }
    }
}
