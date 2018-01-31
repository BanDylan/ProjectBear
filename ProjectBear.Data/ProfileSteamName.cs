using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBear.Data
{
    public class ProfileSteamName
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProfileSteamNameId { get; set; }

        [Required]
        public Guid ProfileId { get; set; }

        [Required]
        public string SteamName { get; set; }

        [Required]
        public DateTime FirstUsedDate { get; set; }


        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
    }
}
