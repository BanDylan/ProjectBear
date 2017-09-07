using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class Roster
    {
        public Roster()
        {
            TimeSlots = new HashSet<TimeSlot>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RosterId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsPublished { get; set; }
        [Required]
        public bool IsTemplate { get; set; }



        public virtual ICollection<TimeSlot> TimeSlots { get; set; }
    }
}
