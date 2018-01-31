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

namespace ProjectBear.CMS.Modules.Content.RosterManagement
{
    [RoutePrefix("Content/RosterManagement"), Route("{action=index}")]
    public class RosterManagementController : Controller
    {
        ProjectBearDataContext db = new ProjectBearDataContext();
        public RosterManagementViewModel GetSession()
        {
            if (Session["CurrentRoster"] == null)
                Session["CurrentRoster"] = new RosterManagementViewModel();
            return (RosterManagementViewModel)Session["CurrentRoster"];
        }

        public void SetSession(RosterManagementViewModel model)
        {
            Session["CurrentRoster"] = model;
        }


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

        [HttpPost]
        public bool SaveSelectedTemplateId(Guid selectedTemplateId)
        {
            Session["SelectedTemplateId"] = selectedTemplateId;
            return true;
        }

        public ActionResult Add()
        {
            var viewModel = new RosterManagementViewModel
            {
                Date = DateTime.Now.Date.AddHours(16),
                IsPublished = false,
                TimeSlots = new List<TimeSlot>(),
            };

            if (Session["SelectedTemplateId"] == null || (Guid)Session["SelectedTemplateId"] == Guid.Empty) {
                viewModel.TimeSlots.Add(new TimeSlot {
                    NumberOfPlayers = 0,
                    NumberOfReserves = 0,
                    Length = 60,
                });
            }
            else 
            {
                var id = (Guid)Session["SelectedTemplateId"];
                var template = db.RosterTemplate.SingleOrDefault(m => m.RosterTemplateId == id);
                
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
            }
             

            SetSession(viewModel);
            return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", viewModel);
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
                SetSession(model);
                return View("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", model);
            }
            return View("~/Modules/Content/RosterManagement/RosterManagementIndex.cshtml");
        }

        
        [HttpGet, PageAuthorize("Administration"), Route("Delete")]
        public ActionResult Delete(Guid id)
        {
            if (id != null)
            {
                Roster roster = new Roster
                {
                    RosterId = id
                };
                db.Entry(roster).State = EntityState.Deleted;
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
            return RedirectToAction("Index", "RosterManagement");
        }


        #region Form

        [HttpGet]
        public ActionResult Roster()
        {
            var roster = GetSession();
            return PartialView("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", roster);
        }


        [HttpPost]
        public bool SetStartTime(string value)
        {
            var roster = GetSession();
            try
            {
                roster.Date = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                roster.Edited = true;
                SetSession(roster);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        [HttpPost]
        public ActionResult AddTimeSlot()
        {
            var roster = GetSession();
            roster.TimeSlots.Add(new TimeSlot()
            {
                NumberOfPlayers = 0,
                NumberOfReserves = 0,
                Length = 60,
            });
            roster = UpdateOffsets(roster);
            roster.Edited = true;
            SetSession(roster);
            return PartialView("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", roster);
        }

        [HttpPost]
        public ActionResult DeleteTimeSlot(int index)
        {
            var roster = GetSession();
            if (index < roster.TimeSlots.Count)
            {
                roster.TimeSlots.RemoveAt(index);
            }
            roster = UpdateOffsets(roster);
            roster.Edited = true;
            SetSession(roster);
            return PartialView("~/Modules/Content/RosterManagement/RosterManagementForm.cshtml", roster);
        }

        private RosterManagementViewModel UpdateOffsets(RosterManagementViewModel roster)
        {
            int cumulative = 0;
            foreach (var timeSlot in roster.TimeSlots)
            {
                timeSlot.Offset = cumulative;
                cumulative += timeSlot.Length;
            }
            return roster;
        }

        #region TimeSlotForm

        [HttpPost]
        public bool SetTimeSlotGameName(string value, int index)
        {
            var roster = GetSession();
            roster.TimeSlots[index].GameName = value;
            roster.Edited = true;
            SetSession(roster);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotLength(int value, int index)
        {
            var roster = GetSession();
            roster.TimeSlots[index].Length = value;
            roster = UpdateOffsets(roster);
            roster.Edited = true;
            SetSession(roster);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotPlayerCount(int value, int index)
        {
            var roster = GetSession();
            roster.TimeSlots[index].NumberOfPlayers = value;
            roster.Edited = true;
            SetSession(roster);
            return true;
        }

        [HttpPost]
        public bool SetTimeSlotReserveCount(int value, int index)
        {
            var roster = GetSession();
            roster.TimeSlots[index].NumberOfReserves = value;
            roster.Edited = true;
            SetSession(roster);
            return true;
        }

        [HttpPost]
        public bool SaveRoster()
        {
            var roster = GetSession();

            if (roster.RosterId == Guid.Empty)
            {
                var model = new Roster
                {
                    Date = roster.Date,
                    IsPublished = false,
                };
                db.Roster.Add(model);
                db.SaveChanges();
                roster.RosterId = model.RosterId;
                foreach (var timeSlot in roster.TimeSlots)
                {
                    timeSlot.RosterId = model.RosterId;
                    db.TimeSlot.Add(timeSlot);
                }
                db.SaveChanges();
            }
            else
            {
                var model = new Roster
                {
                    RosterId = roster.RosterId,
                    Date = roster.Date,
                    IsPublished = false,
                };
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var oldTimeSlot in db.TimeSlot.Where(x => x.RosterId == roster.RosterId))
                {
                    db.Entry(oldTimeSlot).State = EntityState.Deleted;
                }

                foreach (var newTimeSlot in roster.TimeSlots)
                {
                    var timeSlotModel = new TimeSlot
                    {
                        RosterId = roster.RosterId,
                        GameName = newTimeSlot.GameName,
                        Length = newTimeSlot.Length,
                        Offset = newTimeSlot.Offset,
                        NumberOfPlayers = newTimeSlot.NumberOfPlayers,
                        NumberOfReserves = newTimeSlot.NumberOfReserves,
                    };
                    db.TimeSlot.Add(timeSlotModel);
                }
                db.SaveChanges();
            }
            roster.Edited = false;

            SetSession(roster);
            return true;
        }

        #endregion TimeSlotForm

        #endregion Form
    }
}