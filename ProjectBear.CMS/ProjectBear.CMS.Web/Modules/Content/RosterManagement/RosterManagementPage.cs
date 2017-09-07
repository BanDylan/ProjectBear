using Serenity.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectBear.Data;
using ProjectBear.CMS.ViewModels;

namespace ProjectBear.CMS.Modules.Content.RosterManagement
{
    [RoutePrefix("Content/RosterManagement"), Route("{action=index}")]
    public class RosterManagementController : Controller
    {
        ProjectBearDataContext db = new ProjectBearDataContext();

        [Authorize, HttpGet, Route("~/")]//[HttpGet, PageAuthorize("Administration"), Route("Index")]
        public ActionResult Index()
        {
            var rosters = db.Roster.ToList();
            var viewList = new List<RosterManagementViewModel>();

            foreach (var item in rosters)
            {
                var iView = new RosterManagementViewModel
                {

                };
                viewList.Add(iView);
            }
            return View("~/Modules/Content/RosterManagement/RosterManagementIndex.cshtml", viewList);
        }

        [HttpGet, PageAuthorize("Administration"), Route("Add")]
        public ActionResult Add()
        {
            return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", new RosterManagementViewModel());
        }

        [HttpGet, PageAuthorize("Administration"), Route("Edit")]
        public ActionResult Edit(Guid? id)
        {
            Roster roster = db.Roster.SingleOrDefault(m => m.RosterId == id);
            if (roster != null)
            {
                RosterManagementViewModel model = new RosterManagementViewModel
                {

                };
                return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", model);
            }
            return View("~/Modules/Content/RosterManagement/RosterManagementIndex.cshtml");
        }

        [HttpPost, PageAuthorize("Administration"), Route("Save")]
        public ActionResult Save(RosterManagementViewModel model, HttpPostedFileBase upload)
        {
            string path = "", filePath = "";
            Roster dataModel = new Roster();
            if (model.RosterId == null || model.RosterId == Guid.Empty)
            {
                dataModel.RosterId = Guid.NewGuid();
            }
            if (ModelState.IsValid)
            {
                //Save to db or update
                if (model.RosterId != null && model.RosterId != Guid.Empty)
                {
                    //Update

                }
                else
                {

                }
                db.SaveChanges();
                //Upload file

                return RedirectToAction("Index", "RosterManagement");
            }
            return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", model);
        }

        [HttpGet, PageAuthorize("Administration"), Route("Delete")]
        public ActionResult Delete(Guid id)
        {
            if (id != null)
            {
                Roster template = new Roster
                {
                    RosterId = id
                };
                db.Entry(template).State = EntityState.Deleted;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "SubscriptionManagement");
        }

        [HttpGet, PageAuthorize("Administration"), Route("Delete")]
        public ActionResult Publish(Guid id)
        {
            Roster roster = db.Roster.SingleOrDefault(m => m.RosterId == id);
            if (roster != null)
            {
                roster.IsPublished = true;
                db.Entry(roster).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View("~/Modules/Content/RosterManagement/RosterManagementIndex.cshtml");
        }
    }
}