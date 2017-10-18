using ProjectBear.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBear.CMS.ViewModels
{
    public class RosterTemplateManagementViewModel
    {
        public RosterTemplateManagementViewModel()
        {
            TimeSlots = new List<TimeSlotTemplate>();
            TimeSlots.Add(new TimeSlotTemplate()
            {
                NumberOfPlayers = 1,
                NumberOfReserves = 1,
                Length = 60,
                Offset = 0,
            });
        }

        public Guid RosterTemplateId { get; set; }

        public string TemplateName { get; set; }
        public List<TimeSlotTemplate> TimeSlots { get; set; }

        public int TimeSlotCount => TimeSlots != null ? TimeSlots.Count : 0;

        public bool Edited { get; set; } = false;
    }
}