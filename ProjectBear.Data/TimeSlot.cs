using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class TimeSlot
    {
        public TimeSlot()
        {
            Players = new HashSet<PlayerInTimeSlot>();
            Reserves = new HashSet<ReserveInTimeSlot>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TimeSlotId { get; set; }
        [Required]
        public Guid RosterId { get; set; }
        [Required]
        public int Length { get; set; } //In Min
        [Required]
        public int Offset { get; set; } // In Min
        [Required]
        public string GameName { get; set; }
        [Required]
        public bool IsSteamGame { get; set; } = false;
        [Required]
        public int NumberOfPlayers { get; set; }
        

        public virtual ICollection<PlayerInTimeSlot> Players { get; set; }
        public virtual ICollection<ReserveInTimeSlot> Reserves { get; set; }
        [ForeignKey("RosterId")]
        public virtual Roster Roster { get; set; }
    }
}
