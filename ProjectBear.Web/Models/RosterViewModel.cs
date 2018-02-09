using ProjectBear.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBear.Web.Models
{
    public class RosterViewModel
    {
        public RosterViewModel()
        {
            TimeSlots = new List<TimeSlotViewModel>();
        }

        public RosterViewModel(Roster roster, Guid? currentUserProfileId = null)
        {
            TimeSlots = new List<TimeSlotViewModel>();
            RosterId = roster.RosterId;
            Date = roster.Date;

            foreach(var timeSlot in roster.TimeSlots.OrderBy(x => x.Offset))
            {
                TimeSlots.Add(new TimeSlotViewModel(timeSlot, Date, currentUserProfileId));
            }
        }

        public Guid RosterId { get; set; }

        public DateTime Date { get; set; }


        public List<TimeSlotViewModel> TimeSlots { get; set; }
    }
}