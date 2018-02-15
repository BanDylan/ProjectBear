using Serenity.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectBear.Data;
using ProjectBear.CMS.ViewModels;
using System.Globalization;

namespace ProjectBear.CMS.Modules.Content.PlayerManagement
{
    [RoutePrefix("Content/PlayerManagement"), Route("{action=index}")]
    public class PlayerManagementController : Controller
    {
        ProjectBearDataContext db = new ProjectBearDataContext();


        [PageAuthorize("Administration")]
        public ActionResult Index(string name = "")
        {
            var playerList = new List<PlayerViewModel>();

            var steamNameProfiles = db.ProfileSteamName.Where(x => x.SteamName.Contains(name));
            foreach (var profile in steamNameProfiles)
            {
                if (!playerList.Any(x => x.ProfileId == profile.ProfileId))
                    playerList.Add(new PlayerViewModel(profile.ProfileId, profile.SteamName, name == ""));
            }

            var nonSteamProfiles = db.PlayersInTimeSlot.Where(x => x.NonSteamName.Contains(name));
            foreach (var profile in nonSteamProfiles)
            {
                if (!playerList.Any(x => x.ProfileId == profile.ProfileId))
                    playerList.Add(new PlayerViewModel(profile.ProfileId, profile.NonSteamName, name == ""));
            }

            playerList = playerList.OrderByDescending(x => x.GameBookings.FirstOrDefault()?.Date ?? new DateTime()).ToList();
            return View("~/Modules/Content/PlayerManagement/PlayerManagementIndex.cshtml", playerList);
        }


        [HttpGet, PageAuthorize("Administration")]
        public ActionResult ViewPlayer(Guid id)
        {
            var player = new PlayerViewModel(id, "", true, true);
            return View("~/Modules/Content/PlayerManagement/PlayerManagementForm.cshtml", player);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageAuthorize("Administration")]
        public bool ToggleBan(Guid profileId)
        {
            var profile = db.Profile.FirstOrDefault(x => x.ProfileId == profileId);
            profile.Banned = !profile.Banned;
            if(profile.Banned)
            {
                var now = DateTime.Now;
                var bookings = profile.PlayerSlots.Where(x => x.TimeSlot.Roster.Date.AddMinutes(x.TimeSlot.Offset) >= now).ToList();
                for(int i = 0; i < bookings.Count; i++)
                {
                    db.Entry(bookings[i]).State = EntityState.Deleted;
                }
 
                var reserves = profile.ReserveSlots.Where(x => x.TimeSlot.Roster.Date.AddMinutes(x.TimeSlot.Offset) >= now).ToList();
                for (int i = 0; i < reserves.Count; i++)
                {
                    db.Entry(reserves[i]).State = EntityState.Deleted;
                }
            }
            db.Entry(profile).State = EntityState.Modified;
            db.SaveChanges();
            return profile.Banned;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageAuthorize("Administration")]
        public ActionResult AddStrike(Guid profileId, string reason)
        {
            var strike = new ProfileStrike
            {
                ProfileId = profileId,
                DateIssued = DateTime.Now,
                Reason = reason,
            };

            db.ProfileStrike.Add(strike);
            db.SaveChanges();

            var player = db.Profile.FirstOrDefault(x => x.ProfileId == profileId);
            return View("~/Modules/Content/PlayerManagement/PlayerManagementForm.cshtml", player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageAuthorize("Administration")]
        public ActionResult RemoveStrike(Guid profileStrikeId)
        {
            var strike = db.ProfileStrike.FirstOrDefault(x => x.ProfileStrikeId == profileStrikeId);
            var player = strike.Profile;
            db.Entry(strike).State = EntityState.Deleted;
            db.SaveChanges();
            return View("~/Modules/Content/PlayerManagement/PlayerManagementForm.cshtml", player);
        }

    }
}