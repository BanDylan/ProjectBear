namespace ProjectBear.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerInTimeSlots",
                c => new
                    {
                        PlayerInTimeSlotId = c.Guid(nullable: false, identity: true),
                        ProfileId = c.Guid(nullable: false),
                        TimeSlotId = c.Guid(nullable: false),
                        SteamName = c.String(nullable: false),
                        TwitchName = c.String(nullable: false),
                        SignUpTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerInTimeSlotId)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.TimeSlots", t => t.TimeSlotId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.TimeSlotId);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ProfileId = c.Guid(nullable: false, identity: true),
                        AspUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ProfileId)
                .ForeignKey("dbo.AspNetUsers", t => t.AspUserId, cascadeDelete: true)
                .Index(t => t.AspUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ReserveInTimeSlots",
                c => new
                    {
                        ReserveInTimeSlotId = c.Guid(nullable: false, identity: true),
                        ProfileId = c.Guid(nullable: false),
                        TimeSlotId = c.Guid(nullable: false),
                        SteamName = c.String(nullable: false),
                        TwitchName = c.String(nullable: false),
                        SignUpTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReserveInTimeSlotId)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.TimeSlots", t => t.TimeSlotId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.TimeSlotId);
            
            CreateTable(
                "dbo.TimeSlots",
                c => new
                    {
                        TimeSlotId = c.Guid(nullable: false, identity: true),
                        RosterId = c.Guid(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        GameName = c.String(nullable: false),
                        NumberOfPlayers = c.Int(nullable: false),
                        NumberOfReserves = c.Int(nullable: false),
                        RosterTemplate_RosterTemplateId = c.Guid(),
                    })
                .PrimaryKey(t => t.TimeSlotId)
                .ForeignKey("dbo.Rosters", t => t.RosterId, cascadeDelete: true)
                .ForeignKey("dbo.RosterTemplates", t => t.RosterTemplate_RosterTemplateId)
                .Index(t => t.RosterId)
                .Index(t => t.RosterTemplate_RosterTemplateId);
            
            CreateTable(
                "dbo.Rosters",
                c => new
                    {
                        RosterId = c.Guid(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RosterId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.RosterTemplates",
                c => new
                    {
                        RosterTemplateId = c.Guid(nullable: false, identity: true),
                        RosterName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RosterTemplateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSlots", "RosterTemplate_RosterTemplateId", "dbo.RosterTemplates");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TimeSlots", "RosterId", "dbo.Rosters");
            DropForeignKey("dbo.ReserveInTimeSlots", "TimeSlotId", "dbo.TimeSlots");
            DropForeignKey("dbo.PlayerInTimeSlots", "TimeSlotId", "dbo.TimeSlots");
            DropForeignKey("dbo.ReserveInTimeSlots", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.PlayerInTimeSlots", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "AspUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TimeSlots", new[] { "RosterTemplate_RosterTemplateId" });
            DropIndex("dbo.TimeSlots", new[] { "RosterId" });
            DropIndex("dbo.ReserveInTimeSlots", new[] { "TimeSlotId" });
            DropIndex("dbo.ReserveInTimeSlots", new[] { "ProfileId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Profiles", new[] { "AspUserId" });
            DropIndex("dbo.PlayerInTimeSlots", new[] { "TimeSlotId" });
            DropIndex("dbo.PlayerInTimeSlots", new[] { "ProfileId" });
            DropTable("dbo.RosterTemplates");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Rosters");
            DropTable("dbo.TimeSlots");
            DropTable("dbo.ReserveInTimeSlots");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Profiles");
            DropTable("dbo.PlayerInTimeSlots");
        }
    }
}
