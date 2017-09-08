namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTimeSlotsToUseDurationRatherThanFixedTimes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSlots", "Length", c => c.Int(nullable: false));
            AddColumn("dbo.TimeSlots", "Offset", c => c.Int(nullable: false));
            DropColumn("dbo.TimeSlots", "StartTime");
            DropColumn("dbo.TimeSlots", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TimeSlots", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.TimeSlots", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.TimeSlots", "Offset");
            DropColumn("dbo.TimeSlots", "Length");
        }
    }
}
