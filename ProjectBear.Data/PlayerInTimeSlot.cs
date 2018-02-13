using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class PlayerInTimeSlot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PlayerInTimeSlotId { get; set; }
        [Required]
        public Guid ProfileId { get; set; }
        [Required]
        public Guid TimeSlotId { get; set; }

        public string NonSteamName { get; set; }
        [Required]
        public DateTime SignUpTime { get; set; }

        [Required]
        public bool DidNotPitch { get; set; } = false;

        [Required]
        public bool PromotedFromReserve { get; set; } = false;

        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
        [ForeignKey("TimeSlotId")]
        public virtual TimeSlot TimeSlot { get; set; }
    }
}
