namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDidNotPitchToPlayerInTimeSlot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerInTimeSlots", "DidNotPitch", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlayerInTimeSlots", "DidNotPitch");
        }
    }
}
