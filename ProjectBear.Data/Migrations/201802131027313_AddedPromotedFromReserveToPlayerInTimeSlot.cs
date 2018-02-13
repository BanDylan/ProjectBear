namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPromotedFromReserveToPlayerInTimeSlot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerInTimeSlots", "PromotedFromReserve", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlayerInTimeSlots", "PromotedFromReserve");
        }
    }
}
