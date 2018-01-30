using ProjectBear.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBear.Web.Models
{
    public class TimeSlotViewModel
    {
        public TimeSlotViewModel(TimeSlot timeSlot, DateTime rosterStartTime)
        {
            Players = new List<UserViewModel>();
            Reserves = new List<UserViewModel>();

            TimeSlotId = timeSlot.TimeSlotId;
            RosterId = timeSlot.RosterId;

            StartTime = rosterStartTime.AddMinutes(timeSlot.Offset);
            EndTime = StartTime.AddMinutes(timeSlot.Length);

            NumberOfPlayers = timeSlot.NumberOfPlayers;
            NumberOfReserves = timeSlot.NumberOfReserves;

            foreach(var player in timeSlot.Players)
            {
                Players.Add(new UserViewModel(player));
            }

            foreach (var reserve in timeSlot.Reserves)
            {
                Reserves.Add(new UserViewModel(reserve));
            }
        }

        public Guid TimeSlotId { get; set; }
        public Guid RosterId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string GameName { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfReserves { get; set; }

        public bool IsBookable {
            get
            {
                return DateTime.Now > StartTime;
            }
        }

        public List<UserViewModel> Players { get; set; }
        public List<UserViewModel> Reserves { get; set; }

    }
}