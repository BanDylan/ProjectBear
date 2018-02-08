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
            IsSteamGame = timeSlot.IsSteamGame;


            StartTime = rosterStartTime.AddMinutes(timeSlot.Offset);
            EndTime = StartTime.AddMinutes(timeSlot.Length);

            NumberOfPlayers = timeSlot.NumberOfPlayers;

            foreach(var player in timeSlot.Players)
            {
                Players.Add(new PlayerViewModel(player, IsSteamGame, currentUserProfileId));
            }

            foreach (var reserve in timeSlot.Reserves)
            {
                Reserves.Add(new PlayerViewModel(reserve, IsSteamGame, currentUserProfileId));
            }

            BookPlayerCheck();
            BookReserveCheck();
        }

        public Guid TimeSlotId { get; set; }
        public Guid RosterId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string GameName { get; set; }
        public bool IsSteamGame { get; set; }
        public int NumberOfPlayers { get; set; }
        public List<PlayerViewModel> Players { get; set; }
        public List<PlayerViewModel> Reserves { get; set; }


        public bool BookingButtonsEnabled => DateTime.Now < StartTime;
        public bool IsPlayerBooked => Players.Any(x => x.MatchesLoggedInUser);
        public bool IsReserveBooked => Reserves.Any(x => x.MatchesLoggedInUser);

        public bool CanBookPlayer { get; set; }
        public bool CanBookReserve { get; set; }
        public string DenyPlayerBookReason { get; set; }
        public string DenyReserveBookReason { get; set; }


        private void BookPlayerCheck ()
        {
            if (_currentUserProfileId != null)
            {
                if (!db.Profile.FirstOrDefault(x => x.ProfileId == _currentUserProfileId).Banned)
                {
                    if(NumberOfPlayers != Players.Count)
                    {
                        if (!db.PlayersInTimeSlot.Any(x => x.ProfileId == _currentUserProfileId && x.TimeSlot.RosterId == RosterId))
                        {
                            DenyPlayerBookReason = "";
                            CanBookPlayer = true;
                            return;
                        }
                        else
                            DenyPlayerBookReason = "Only one Player booking per stream is allowed. You can however book as a Reserve for as many time slots as you wish. If the player slots aren't filled, or someone doesn't pitch, you may be upgraded to a Player for that time slot.";
                    }
                    else
                        DenyPlayerBookReason = "This time slot has been fully booked.";
                }
                else
                    DenyPlayerBookReason = "Your profile has been banned.";
            }
            else
                DenyPlayerBookReason = "You must be signed in to make a booking.";

            CanBookPlayer = false;
        }

        private void BookReserveCheck()
        {
            if (_currentUserProfileId != null)
            {
                if (!db.Profile.FirstOrDefault(x => x.ProfileId == _currentUserProfileId).Banned)
                {
                    if (!IsPlayerBooked)
                    {
                        DenyReserveBookReason = "";
                        CanBookReserve = true;
                        return;
                    }
                    else
                        DenyReserveBookReason = "You already have a guarenteed spot.";
                }
                else
                    DenyReserveBookReason = "Your profile has been banned.";
            }
            else
                DenyReserveBookReason = "You must be signed in to make a booking.";

            CanBookReserve = false;
        }
    }
}