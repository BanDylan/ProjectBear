using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using System.Web;
using System.Threading.Tasks;
using ProjectBear.Data;
using ProjectBear.Web.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System;

namespace ProjectBear.Web.Controllers
{
    public class AccountController : Controller
    {
        ProjectBearDataContext db = new ProjectBearDataContext();

        public AccountController()
        {
        }
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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


        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // https://stackoverflow.com/questions/20737578/asp-net-sessionid-owin-cookies-do-not-send-to-browser
            //Session["Workaround"] = 0;

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return View("Error");// RedirectToAction("Index", "Home");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    SteamNameUpdateCheck(loginInfo);
                    return RedirectToAction("Rosters", "Home");
                //case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel() { Username = loginInfo?.DefaultUserName ?? "" } );
            }
        }

        private void SteamNameUpdateCheck(ExternalLoginInfo loginInfo)
        {
            var aspUserId = UserManager.Users.FirstOrDefault(y => y.Logins.Any(z => z.ProviderKey == loginInfo.Login.ProviderKey))?.Id;
            if(aspUserId != null)
            {
                var profileId = db.Profile.FirstOrDefault(x => x.AspUserId == aspUserId)?.ProfileId;
                if(profileId != null)
                {
                    var lastRecordedPlayerName = db.ProfileSteamName.Where(x => x.ProfileId == profileId).OrderByDescending(x => x.FirstUsedDate).FirstOrDefault()?.SteamName;

                    if (lastRecordedPlayerName != loginInfo.DefaultUserName)
                    {
                        db.ProfileSteamName.Add(new ProfileSteamName
                        {
                            ProfileId = profileId.Value,
                            SteamName = loginInfo.DefaultUserName,
                            FirstUsedDate = DateTime.Now,
                        });
                        db.SaveChanges();
                    }
                }              
            }          
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView(model);
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return PartialView("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = Guid.NewGuid().ToString() };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    var profile = new Profile
                    {
                        //EmailAddress = model.Email,
                        AspUserId = user.Id,
                    };
                    db.Profile.Add(profile);
                    db.SaveChanges();
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        SteamNameUpdateCheck(info);
                        return RedirectToAction("Index", "Home");
                    }
                }
                AddErrors(new IdentityResult(result.Errors.Where(x => !(x.Contains("Name") && x.Contains("is already taken.")))));
            }

            ViewBag.ReturnUrl = returnUrl;
            return PartialView(model);
        }


        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

    }
}