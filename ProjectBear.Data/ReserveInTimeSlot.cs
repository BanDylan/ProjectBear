using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class ReserveInTimeSlot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ReserveInTimeSlotId { get; set; }
        [Required]
        public Guid ProfileId { get; set; }
        [Required]
        public Guid TimeSlotId { get; set; }

        [Required]
        public string SteamName { get; set; }

        [Required]
        public string TwitchName { get; set; }

        [Required]
        public DateTime SignUpTime { get; set; }


        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
        [ForeignKey("TimeSlotId")]
        public virtual TimeSlot TimeSlot { get; set; }
    }
}
