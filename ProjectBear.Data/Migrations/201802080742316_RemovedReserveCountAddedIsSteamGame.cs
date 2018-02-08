namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedReserveCountAddedIsSteamGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSlots", "IsSteamGame", c => c.Boolean(nullable: false));
            AddColumn("dbo.TimeSlotTemplates", "IsSteamGame", c => c.Boolean(nullable: false));
            DropColumn("dbo.TimeSlots", "NumberOfReserves");
            DropColumn("dbo.TimeSlotTemplates", "NumberOfReserves");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TimeSlotTemplates", "NumberOfReserves", c => c.Int(nullable: false));
            AddColumn("dbo.TimeSlots", "NumberOfReserves", c => c.Int(nullable: false));
            DropColumn("dbo.TimeSlotTemplates", "IsSteamGame");
            DropColumn("dbo.TimeSlots", "IsSteamGame");
        }
    }
}
