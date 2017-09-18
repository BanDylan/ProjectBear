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
            var viewModel = new RosterManagementIndexViewModel()
            {
                RosterList = new List<RosterManagementViewModel>(),
                TemplateList = new List<RosterTemplateManagementViewModel>(),
            };

            foreach (var roster in rosters)
            {
                var model = new RosterManagementViewModel
                {
                    RosterId = roster.RosterId,
                    Date = roster.Date,
                    IsPublished = roster.IsPublished,
                    TimeSlots = roster.TimeSlots.ToList(),
                };
                viewModel.RosterList.Add(model);
            }

            var templates = db.RosterTemplate.ToList();
            viewModel.TemplateList.Add(new RosterTemplateManagementViewModel
            {
                TemplateName = "Blank roster",
                TimeSlots = new List<TimeSlotTemplate>(),
            });
            viewModel.SelectedTemplate = viewModel.TemplateList.First();

            foreach(var template in templates)
            {
                viewModel.TemplateList.Add(new RosterTemplateManagementViewModel
                {
                    RosterTemplateId = template.RosterTemplateId,
                    TemplateName = template.RosterName,
                    TimeSlots = template.TimeSlots.ToList(),
                });
            }

            return View("~/Modules/Content/RosterManagement/RosterManagementIndex.cshtml", viewModel);
        }

        [HttpPost, PageAuthorize("Administration"), Route("Add")]
        public ActionResult Add(Guid id)
        {
            if (id == Guid.Empty)
                return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", new RosterManagementViewModel());
            else
            {
                var template = db.RosterTemplate.SingleOrDefault(m => m.RosterTemplateId == id);
                var viewModel = new RosterManagementViewModel()
                {
                    TimeSlots = new List<TimeSlot>(),
                };
                foreach(var timeSlot in template.TimeSlots)
                {
                    viewModel.TimeSlots.Add(new TimeSlot
                    {
                        GameName = timeSlot.GameName,
                        Length = timeSlot.Length,
                        Offset = timeSlot.Offset,
                        NumberOfPlayers = timeSlot.NumberOfPlayers,
                        NumberOfReserves = timeSlot.NumberOfReserves,
                    });
                }

                return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", viewModel);
            }         
        }

        [HttpGet, PageAuthorize("Administration"), Route("Edit")]
        public ActionResult Edit(Guid? id)
        {
            Roster roster = db.Roster.SingleOrDefault(m => m.RosterId == id);
            if (roster != null)
            {
                RosterManagementViewModel model = new RosterManagementViewModel
                {
                    RosterId = roster.RosterId,
                    Date = roster.Date,
                    IsPublished = roster.IsPublished,
                    TimeSlots = roster.TimeSlots.ToList(),
                };
                return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", model);
            }
            return View("~/Modules/Content/RosterManagement/RosterManagementIndex.cshtml");
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
            return RedirectToAction("Index", "RosterManagement");
        }

        [HttpGet, PageAuthorize("Administration"), Route("Publish")]
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