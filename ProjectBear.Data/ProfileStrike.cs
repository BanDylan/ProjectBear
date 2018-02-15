using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class ProfileStrike
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProfileStrikeId { get; set; }
        [Required]
        public Guid ProfileId { get; set; }
        [Required]
        public string Reason { get; set; }

        public DateTime DateIssued { get; set; }
        

        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
    }
}
