using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ProjectBear.Data
{
    public class ProjectBearDataContext : IdentityDbContext<ApplicationUser>
    {
        public ProjectBearDataContext()
            : base("Default", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjectBearDataContext, Migrations.Configuration>("Default"));
        }

        public static ProjectBearDataContext Create()
        {
            return new ProjectBearDataContext();
        }


        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<ProfileSteamName> ProfileSteamName { get; set; }

        public virtual DbSet<Roster> Roster { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }

        public virtual DbSet<PlayerInTimeSlot> PlayersInTimeSlot { get; set; }
        public virtual DbSet<ReserveInTimeSlot> ReservesInTimeSlot { get; set; }

        public virtual DbSet<RosterTemplate> RosterTemplate { get; set; }

        public virtual DbSet<TimeSlotTemplate> TimeSlotTemplate { get; set; }

    }
}
