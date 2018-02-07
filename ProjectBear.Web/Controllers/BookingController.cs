using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProjectBear.Data;
using ProjectBear.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public ActionResult Rosters()
        {
            string userId = User?.Identity?.GetUserId();
            Guid? profileId = db.Profile.SingleOrDefault(m => m.AspUserId == userId)?.ProfileId;

            List<RosterViewModel> rosters = new List<RosterViewModel>();
            var dbRosters = db.Roster.Where(x => x.IsPublished).ToList();
            dbRosters = dbRosters.Where(x => x.Date > (DateTime.Now).AddDays(-1.0)).OrderBy(x => x.Date).ToList();

            foreach (var dbRoster in dbRosters)
            {
                rosters.Add(new RosterViewModel(dbRoster, profileId));
            }
  
            return View(rosters);
        }
    }
}