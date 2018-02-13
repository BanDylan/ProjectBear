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
                if (!playerList.Any(x => x.PlayerProfile.ProfileId == profile.ProfileId))
                    playerList.Add(new PlayerViewModel(profile.ProfileId, profile.SteamName, name == ""));
            }

            var nonSteamProfiles = db.PlayersInTimeSlot.Where(x => x.NonSteamName.Contains(name));
            foreach (var profile in nonSteamProfiles)
            {
                if (!playerList.Any(x => x.PlayerProfile.ProfileId == profile.ProfileId))
                    playerList.Add(new PlayerViewModel(profile.ProfileId, profile.NonSteamName, name == ""));
            }

            playerList = playerList.OrderByDescending(x => x.GameBookings.FirstOrDefault()?.Date ?? new DateTime()).ToList();
            return View("~/Modules/Content/PlayerManagement/PlayerManagementIndex.cshtml", playerList);
        }


        [HttpGet, PageAuthorize("Administration")]
        public ActionResult ViewPlayer(Guid id)
        {
            var player = new PlayerViewModel(id, "", true);
            return View("~/Modules/Content/PlayerManagement/PlayerManagementForm.cshtml", player);
        }

    }
}