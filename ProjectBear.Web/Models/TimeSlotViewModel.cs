using ProjectBear.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBear.Web.Models
{
    public class TimeSlotViewModel
    {
        ProjectBearDataContext db = new ProjectBearDataContext();
        private Guid? _currentUserProfileId;
        public TimeSlotViewModel(TimeSlot timeSlot, DateTime rosterStartTime, Guid? currentUserProfileId = null)
        {
            _currentUserProfileId = currentUserProfileId;
            Players = new List<PlayerViewModel>();
            Reserves = new List<PlayerViewModel>();

            TimeSlotId = timeSlot.TimeSlotId;
            RosterId = timeSlot.RosterId;

            GameName = timeSlot.GameName;

            StartTime = rosterStartTime.AddMinutes(timeSlot.Offset);
            EndTime = StartTime.AddMinutes(timeSlot.Length);

            NumberOfPlayers = timeSlot.NumberOfPlayers;
            NumberOfReserves = timeSlot.NumberOfReserves;

            foreach(var player in timeSlot.Players)
            {
                Players.Add(new PlayerViewModel(player, currentUserProfileId));
            }

            foreach (var reserve in timeSlot.Reserves)
            {
                Reserves.Add(new PlayerViewModel(reserve, currentUserProfileId));
            }
        }

        public Guid TimeSlotId { get; set; }
        public Guid RosterId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string GameName { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfReserves { get; set; }
        public List<PlayerViewModel> Players { get; set; }
        public List<PlayerViewModel> Reserves { get; set; }


        public bool BookingButtonsEnabled => DateTime.Now < StartTime;
        public bool IsPlayerBooked => Players.Any(x => x.MatchesLoggedInUser);
        public bool IsReserveBooked => Reserves.Any(x => x.MatchesLoggedInUser);


        public bool CanBookPlayer {
            get
            {
                if (_currentUserProfileId != null)
                {
                    if (!db.Profile.FirstOrDefault(x => x.ProfileId == _currentUserProfileId).Banned)
                    {
                        if (!db.PlayersInTimeSlot.Any(x => x.ProfileId == _currentUserProfileId && x.TimeSlot.RosterId == RosterId))
                        {
                            DenyPlayerBookReason = "";
                            return true;
                        }
                        else
                            DenyPlayerBookReason = "Only one Player booking per stream is allowed. You can however book as a Reserve for as many time slots as you wish. If the player slots aren't filled, or someone doesn't pitch, you may be upgraded to a Player for that time slot.";
                    }
                    else
                        DenyPlayerBookReason = "Your profile has been banned.";
                }
                else
                    DenyPlayerBookReason = "You must be signed in to make a booking";

                return false;
            }
        }

        public bool CanBookReserve {
            get
            {
                if (_currentUserProfileId != null)
                {
                    if(!db.Profile.FirstOrDefault(x => x.ProfileId == _currentUserProfileId).Banned)
                    {
                        if (!IsPlayerBooked)
                        {
                            DenyReserveBookReason = "";
                            return true;
                        }
                        else
                            DenyReserveBookReason = "You already have a guarenteed spot.";
                    }
                    else
                        DenyReserveBookReason = "Your profile has been banned.";     
                }
                else
                    DenyReserveBookReason = "You must be signed in to make a booking.";

                return false;
            }
        }

        public string DenyPlayerBookReason { get; set; }
        public string DenyReserveBookReason { get; set; }
    }
}