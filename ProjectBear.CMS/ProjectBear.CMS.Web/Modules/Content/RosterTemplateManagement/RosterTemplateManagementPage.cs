using Serenity.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectBear.Data;
using ProjectBear.CMS.ViewModels;

namespace ProjectBear.CMS.Modules.Content.RosterTemplateManagement
{
    [RoutePrefix("Content/RosterTemplateManagement"), Route("{action=index}")]
    public class RosterTemplateManagementController : Controller
    {
        ProjectBearDataContext db = new ProjectBearDataContext();

        public RosterTemplateManagementViewModel GetSession()
        {
            if (Session["CurrentTemplate"] == null)
                Session["CurrentTemplate"] = new RosterTemplateManagementViewModel();
            return (RosterTemplateManagementViewModel)Session["CurrentTemplate"];
        }

        public void SetSession(RosterTemplateManagementViewModel model)
        {
            Session["CurrentTemplate"] = model;
        }

        #region Index

        [HttpGet, PageAuthorize("Administration"), Route("Index")]
        public ActionResult Index()
        {
            var rosters = db.Roster.ToList();
            var viewList = new List<RosterTemplateManagementViewModel>();

            foreach (var item in rosters)
            {
                var iView = new RosterTemplateManagementViewModel
                {

                };
                viewList.Add(iView);
            }
            return View("~/Modules/Content/RosterTemplateManagement/RosterTemplateManagementIndex.cshtml", viewList);
        }

        [HttpGet, PageAuthorize("Administration"), Route("Add")]
        public ActionResult Add()
        {
            SetSession(new RosterTemplateManagementViewModel());
            return View("~/Modules/Content/RosterTemplateManagement/RosterTemplateManagementForm.cshtml", new RosterTemplateManagementViewModel());
        }

        [HttpGet, PageAuthorize("Administration"), Route("Edit")]
        public ActionResult Edit(Guid? id)
        {
            Roster roster = db.Roster.SingleOrDefault(m => m.RosterId == id);
            if (roster != null)
            {
                RosterTemplateManagementViewModel model = new RosterTemplateManagementViewModel
                {

                };
                SetSession(model);
                return View("~/Modules/Content/RosterTemplateManagement/RosterTemplateManagementForm.cshtml", model);
            }
            return View("~/Modules/Content/RosterTemplateManagement/RosterTemplateManagementIndex.cshtml");
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
            return RedirectToAction("Index", "RosterTemplateManagement");
        }

        #endregion Index

        #region Form

        [HttpGet]
        public ActionResult RosterTemplate()
        {
            var template = GetSession();
            return PartialView("~/Modules/Content/RosterTemplateManagement/RosterTemplateManagementForm.cshtml", template);
        }

        [HttpPost]
        public bool SetTemplateName(string name)
        {
            var template = GetSession();
            template.TemplateName = name;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public ActionResult AddTimeSlot()
        {
            var template = GetSession();
            template.TimeSlots.Add(new TimeSlot()
            {
                NumberOfPlayers = 1,
                NumberOfReserves = 1,
                Length = 60,
            });
            template = UpdateOffsets(template);
            SetSession(template);
            return PartialView("~/Modules/Content/RosterTemplateManagement/RosterTemplateManagementForm.cshtml", template);
        }

        [HttpPost]
        public ActionResult DeleteTimeSlot(int index)
        {
            var template = GetSession();
            if (index < template.TimeSlots.Count)
            {
                template.TimeSlots.RemoveAt(index);
            }
            template = UpdateOffsets(template);
            SetSession(template);
            return PartialView("~/Modules/Content/RosterTemplateManagement/RosterTemplateManagementForm.cshtml", template);
        }

        private RosterTemplateManagementViewModel UpdateOffsets(RosterTemplateManagementViewModel template)
        {
            int cumulative = 0;
            foreach (var timeSlot in template.TimeSlots)
            {
                timeSlot.Offset = cumulative;
                cumulative += timeSlot.Length;
            }
            return template;
        }

        #region TimeSlotForm

        [HttpPost]
        public bool SetTimeSlotGameName(string value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].GameName = value;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotLength(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].Length = value;
            template = UpdateOffsets(template);
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotOffset(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].Offset = value;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotPlayerCount(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].NumberOfPlayers = value;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotReserveCount(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].NumberOfReserves = value;
            SetSession(template);
            return true;
        }

        #endregion TimeSlotForm

        #endregion Form

    }
}