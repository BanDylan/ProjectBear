using ProjectBear.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBear.Web.Models
{
    public class PlayerViewModel
    {
        ProjectBearDataContext db = new ProjectBearDataContext();

        public PlayerViewModel(PlayerInTimeSlot user, Guid? currentUserProfileId = null)
        {
            ProfileId = user.ProfileId;
            PlayerName = db.ProfileSteamName.Where(x => x.ProfileId == user.ProfileId).OrderByDescending(x => x.FirstUsedDate).FirstOrDefault().SteamName;
            if (currentUserProfileId.HasValue)
                MatchesLoggedInUser = currentUserProfileId.Value == user.ProfileId;
        }

        public PlayerViewModel(ReserveInTimeSlot user, Guid? currentUserProfileId = null)
        {
            ProfileId = user.ProfileId;
            PlayerName = db.ProfileSteamName.Where(x => x.ProfileId == user.ProfileId).OrderByDescending(x => x.FirstUsedDate).FirstOrDefault().SteamName;
            if (currentUserProfileId.HasValue)
                MatchesLoggedInUser = currentUserProfileId.Value == user.ProfileId;
        }


        public Guid ProfileId { get; set; }
        public string PlayerName { get; set; }
        public bool MatchesLoggedInUser { get; set; }

    }
}