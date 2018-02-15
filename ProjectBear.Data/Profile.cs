using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBear.Data
{
    public class Profile
    {
        public Profile()
        {
            PlayerSlots = new HashSet<PlayerInTimeSlot>();
            ReserveSlots = new HashSet<ReserveInTimeSlot>();
            ProfileSteamNames = new HashSet<ProfileSteamName>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProfileId { get; set; }

        [Required]
        public string AspUserId { get; set; }

        [Required]
        public bool Banned { get; set; } = false;

        public virtual ICollection<PlayerInTimeSlot> PlayerSlots { get; set; }
        public virtual ICollection<ReserveInTimeSlot> ReserveSlots { get; set; }
        public virtual ICollection<ProfileSteamName> ProfileSteamNames { get; set; }
        public virtual ICollection<ProfileStrike> ProfileStrikes { get; set; }

        [ForeignKey("AspUserId")]
        public virtual ApplicationUser AspUser { get; set; }
    }
}
