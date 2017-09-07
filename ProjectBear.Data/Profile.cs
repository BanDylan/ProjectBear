using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class Profile
    {
        public Profile()
        {
            PlayerSlots = new HashSet<PlayerInTimeSlot>();
            ReserveSlots = new HashSet<ReserveInTimeSlot>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProfileId { get; set; }

        [Required]
        public string AspUserId { get; set; }

        public virtual ICollection<PlayerInTimeSlot> PlayerSlots { get; set; }
        public virtual ICollection<ReserveInTimeSlot> ReserveSlots { get; set; }

        public virtual ApplicationUser AspUser { get; set; }
    }
}
