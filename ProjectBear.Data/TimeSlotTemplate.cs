using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBear.Data
{
    public class TimeSlotTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TimeSlotTemplateId { get; set; }
        [Required]
        public Guid RosterTemplateId { get; set; }
        [Required]
        public int Length { get; set; } //In Min
        [Required]
        public int Offset { get; set; } // In Min
        [Required]
        public string GameName { get; set; }
        [Required]
        public int NumberOfPlayers { get; set; }
        [Required]
        public int NumberOfReserves { get; set; }

        [ForeignKey("RosterTemplateId")]
        public virtual RosterTemplate RosterTemplate { get; set; }
    }
}
