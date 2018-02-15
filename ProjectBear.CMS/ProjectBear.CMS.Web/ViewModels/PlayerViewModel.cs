using ProjectBear.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBear.CMS.ViewModels
{
    public class PlayerViewModel
    {
        ProjectBearDataContext db = new ProjectBearDataContext();

        public string CurrentSteamName { get; set; }
        public string MatchingPlayerName { get; set; }
        public Profile PlayerProfile { get; set; }
        public List<PlayerNameViewModel> PlayerNames { get; set; }
        public List<PlayerBookingViewModel> GameBookings { get; set; }

        //public string LastPlayed => GameBookings?.FirstOrDefault()?.Date.ToString("dd/MM/yyyy HH:mm") ?? "Never booked";

        public PlayerViewModel(Guid profileId, string matchingPlayerName, bool showCurrentSteamName, bool getFullData = false)
        {
            CurrentSteamName = db.ProfileSteamName.Where(x => x.ProfileId == profileId).OrderByDescending(y => y.FirstUsedDate).FirstOrDefault().SteamName;
            MatchingPlayerName = showCurrentSteamName ? CurrentSteamName : matchingPlayerName;
            PlayerProfile = db.Profile.FirstOrDefault(x => x.ProfileId == profileId);

            PlayerNames = new List<PlayerNameViewModel>();
            GameBookings = new List<PlayerBookingViewModel>();

            if (getFullData)
            {
                var steamNames = db.ProfileSteamName.Where(x => x.ProfileId == profileId);

                foreach (var steamName in steamNames)
                {
                    if (!PlayerNames.Any(x => x.Name == steamName.SteamName))
                    {
                        PlayerNames.Add(new PlayerNameViewModel
                        {
                            Name = steamName.SteamName,
                        });
                    }

                }
                var nonSteamGameBookings = db.PlayersInTimeSlot.Where(x => x.ProfileId == profileId && !x.TimeSlot.IsSteamGame).ToList();
                foreach (var nonSteamGameBooking in nonSteamGameBookings)
                {
                    if (!PlayerNames.Any(x => x.Name == nonSteamGameBooking.NonSteamName))
                    {
                        PlayerNames.Add(new PlayerNameViewModel
                        {
                            Name = nonSteamGameBooking.NonSteamName,
                        });
                    }
                }
                PlayerNames = PlayerNames.OrderBy(x => x.Name).ToList();

                var bookings = PlayerProfile.PlayerSlots.ToList();//db.PlayersInTimeSlot.Where(x => x.ProfileId == profileId).ToList();
                foreach (var booking in bookings)
                {
                    var bookingStart = booking.TimeSlot.Roster.Date.AddMinutes(booking.TimeSlot.Offset);
                    if (booking.TimeSlot.IsSteamGame)
                    {
                        var name = "";
                        foreach (var steamName in steamNames.OrderBy(x => x.FirstUsedDate))
                        {
                            if (steamName.FirstUsedDate <= bookingStart)
                                name = steamName.SteamName;
                            else
                                break;
                        }

                        GameBookings.Add(new PlayerBookingViewModel
                        {
                            Date = bookingStart,
                            Game = booking.TimeSlot.GameName,
                            PlayerName = name,
                            DidNotPitch = booking.DidNotPitch,
                        });
                    }
                    else
                    {
                        GameBookings.Add(new PlayerBookingViewModel
                        {
                            Date = bookingStart,
                            Game = booking.TimeSlot.GameName,
                            PlayerName = booking.NonSteamName,
                            DidNotPitch = booking.DidNotPitch,
                        });
                    }
                }
                GameBookings = GameBookings.OrderByDescending(x => x.Date).ToList();
            }
            
        }      
    }
}