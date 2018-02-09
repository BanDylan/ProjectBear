using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProjectBear.Data;
using ProjectBear.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectBear.Web.Controllers
{
    public class BookingController : Controller
    {
        ProjectBearDataContext db = new ProjectBearDataContext();

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public BookingController()
        {

        }

        public BookingController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private Guid? UserProfileId ()
        {
            string userId = User?.Identity?.GetUserId();
            Guid? profileId = db.Profile.SingleOrDefault(m => m.AspUserId == userId)?.ProfileId;
            return profileId;
        }

        [HttpGet]
        public ActionResult Rosters()
        {
            List<RosterViewModel> rosters = new List<RosterViewModel>();
            var dbRosters = db.Roster.Where(x => x.IsPublished).ToList();
            dbRosters = dbRosters.Where(x => x.Date > (DateTime.Now).AddDays(-1.0)).OrderBy(x => x.Date).ToList();

            foreach (var dbRoster in dbRosters)
            {
                rosters.Add(new RosterViewModel(dbRoster, UserProfileId()));
            }
  
            return View(rosters);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string AttemptPlayerBooking(Guid timeSlotId, string nonSteamName)
        {
            var timeSlot = db.TimeSlot.FirstOrDefault(x => x.TimeSlotId == timeSlotId);
            var timeSlotBookings = db.PlayersInTimeSlot.Where(x => x.TimeSlotId == timeSlotId).ToList();

            if (UserProfileId().HasValue)
            {
                var profileId = UserProfileId().Value;
                if (!db.Profile.FirstOrDefault(x => x.ProfileId == profileId).Banned)
                {
                    if (timeSlotBookings.Count < timeSlot.NumberOfPlayers)
                    {
                        if (!db.PlayersInTimeSlot.Any(x => x.ProfileId == profileId && x.TimeSlot.RosterId == timeSlot.RosterId))
                        {
                            if (timeSlot.IsSteamGame || (!timeSlot.IsSteamGame && !string.IsNullOrWhiteSpace(nonSteamName)))
                            {
                                var booking = new PlayerInTimeSlot()
                                {
                                    ProfileId = profileId,
                                    SignUpTime = DateTime.Now,
                                    TimeSlotId = timeSlotId,
                                    NonSteamName = nonSteamName,
                                };
                                db.PlayersInTimeSlot.Add(booking);

                                var overLappingReserveBooking = db.ReservesInTimeSlot.FirstOrDefault(x => x.TimeSlotId == timeSlotId && x.ProfileId == profileId);
                                if (overLappingReserveBooking != null)
                                    db.Entry(overLappingReserveBooking).State = EntityState.Deleted;

                                db.SaveChanges();
                                return "success";
                            }
                            else
                                return "Please provide your Player name for this game.";
                            
                        }
                        else
                            return "Only one Player booking per stream is allowed. You can however book as a Reserve for as many time slots as you wish. If the player slots aren't filled, or someone doesn't pitch, you may be upgraded to a Player for that time slot.";
                    }
                    else
                        return "Sorry. All remaining timeslots have since been booked.";
                }
                else
                    return "Your profile has been banned.";
            }
            else
                return "You must be signed in to make a booking.";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string CancelPlayerBooking(Guid timeSlotId)
        {
            if(UserProfileId().HasValue)
            {
                var profileId = UserProfileId().Value;
                var timeSlot = db.TimeSlot.FirstOrDefault(x => x.TimeSlotId == timeSlotId);
                var timeSlotBooking = db.PlayersInTimeSlot.FirstOrDefault(x => x.TimeSlotId == timeSlotId && x.ProfileId == profileId);

                if (timeSlotBooking != null)
                {
                    db.Entry(timeSlotBooking).State = EntityState.Deleted;
                    db.SaveChanges();
                    return "success";
                }
                else
                    return "You don't seem to have a booking for this timeslot.";
            }
            else
                return "You must be signed in to cancel a booking.";       
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string AttemptReserveBooking(Guid timeSlotId, string nonSteamName)
        {
            var timeSlot = db.TimeSlot.FirstOrDefault(x => x.TimeSlotId == timeSlotId);
            var timeSlotBookings = db.PlayersInTimeSlot.Where(x => x.TimeSlotId == timeSlotId).ToList();

            if (UserProfileId().HasValue)
            {
                var profileId = UserProfileId().Value;
                if (!db.Profile.FirstOrDefault(x => x.ProfileId == profileId).Banned)
                {
                    if (!db.PlayersInTimeSlot.Any(x => x.ProfileId == profileId && x.TimeSlot.TimeSlotId == timeSlotId))
                    {
                        if (timeSlot.IsSteamGame || (!timeSlot.IsSteamGame && !string.IsNullOrWhiteSpace(nonSteamName)))
                        {
                            var booking = new ReserveInTimeSlot()
                            {
                                ProfileId = profileId,
                                SignUpTime = DateTime.Now,
                                TimeSlotId = timeSlotId,
                                NonSteamName = nonSteamName,
                            };
                            db.ReservesInTimeSlot.Add(booking);
                            db.SaveChanges();
                            return "success";
                        }
                        else
                            return "Please provide your Player name for this game.";
                    }
                    else
                        return "You already have a guarenteed spot.";
                }
                else
                    return "Your profile has been banned.";
            }
            else
                return "You must be signed in to make a booking.";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string CancelReserveBooking(Guid timeSlotId)
        {
            if (UserProfileId().HasValue)
            {
                var profileId = UserProfileId().Value;
                var timeSlot = db.TimeSlot.FirstOrDefault(x => x.TimeSlotId == timeSlotId);
                var timeSlotBooking = db.ReservesInTimeSlot.FirstOrDefault(x => x.TimeSlotId == timeSlotId && x.ProfileId == profileId);

                if (timeSlotBooking != null)
                {
                    db.Entry(timeSlotBooking).State = EntityState.Deleted;
                    db.SaveChanges();
                    return "success";
                }
                else
                    return "You don't seem to have a booking for this timeslot.";
            }
            else
                return "You must be signed in to cancel a booking.";
        }
    }
}