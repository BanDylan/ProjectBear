using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class RosterTemplate
    {
        public RosterTemplate()
        {
            TimeSlots = new HashSet<TimeSlotTemplate>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RosterTemplateId { get; set; }

        [Required]
        public string RosterName { get; set; }

        public virtual ICollection<TimeSlotTemplate> TimeSlots { get; set; }
    }
}
