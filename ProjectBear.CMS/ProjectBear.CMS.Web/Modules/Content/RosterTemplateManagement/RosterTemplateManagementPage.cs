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
            var templates = db.RosterTemplate.ToList();
            var viewList = new List<RosterTemplateManagementViewModel>();

            foreach (var template in templates)
            {
                var iView = new RosterTemplateManagementViewModel
                {
                    RosterTemplateId = template.RosterTemplateId,
                    TemplateName = template.RosterName,
                    TimeSlots = template.TimeSlots.ToList(),
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
            RosterTemplate template = db.RosterTemplate.SingleOrDefault(m => m.RosterTemplateId == id);
            if (template != null)
            {
                RosterTemplateManagementViewModel model = new RosterTemplateManagementViewModel
                {
                    RosterTemplateId = template.RosterTemplateId,
                    TemplateName = template.RosterName,
                    TimeSlots = template.TimeSlots.ToList(),
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
                RosterTemplate template = new RosterTemplate
                {
                    RosterTemplateId = id
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
        public bool SetTemplateName(string value)
        {
            var template = GetSession();
            template.TemplateName = value;
            template.Edited = true;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public ActionResult AddTimeSlot()
        {
            var template = GetSession();
            template.TimeSlots.Add(new TimeSlotTemplate()
            {
                NumberOfPlayers = 1,
                NumberOfReserves = 1,
                Length = 60,
            });
            template = UpdateOffsets(template);
            template.Edited = true;
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
            template.Edited = true;
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
            template.Edited = true;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotLength(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].Length = value;
            template = UpdateOffsets(template);
            template.Edited = true;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotOffset(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].Offset = value;
            template.Edited = true;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotPlayerCount(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].NumberOfPlayers = value;
            template.Edited = true;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotReserveCount(int value, int index)
        {
            var template = GetSession();
            template.TimeSlots[index].NumberOfReserves = value;
            template.Edited = true;
            SetSession(template);
            return true;
        }

        [HttpPost]
        public bool SaveTemplate()
        {
            var template = GetSession();

            if(template.RosterTemplateId == Guid.Empty)
            {
                var model = new RosterTemplate
                {
                    RosterName = template.TemplateName,
                };
                db.RosterTemplate.Add(model);
                db.SaveChanges();
                template.RosterTemplateId = model.RosterTemplateId;
                foreach (var timeSlot in template.TimeSlots)
                {
                    timeSlot.RosterTemplateId = model.RosterTemplateId;
                    db.TimeSlotTemplate.Add(timeSlot);
                }
                db.SaveChanges();
            } 
            else
            {
                var model = new RosterTemplate
                {
                    RosterTemplateId = template.RosterTemplateId,
                    RosterName = template.TemplateName,
                };
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                foreach(var oldTimeSlot in db.TimeSlotTemplate.Where(x => x.RosterTemplateId == template.RosterTemplateId))
                {
                    db.Entry(oldTimeSlot).State = EntityState.Deleted;
                }

                foreach (var newTimeSlot in template.TimeSlots)
                {
                    var timeSlotModel = new TimeSlotTemplate
                    {
                        RosterTemplateId = template.RosterTemplateId,
                        GameName = newTimeSlot.GameName,
                        Length = newTimeSlot.Length,
                        Offset = newTimeSlot.Offset,
                        NumberOfPlayers = newTimeSlot.NumberOfPlayers,
                        NumberOfReserves = newTimeSlot.NumberOfReserves,
                    };
                    db.TimeSlotTemplate.Add(timeSlotModel);
                }
                db.SaveChanges();
            }
            template.Edited = false;

            SetSession(template);
            return true;
        }

        #endregion TimeSlotForm

        #endregion Form

    }
}