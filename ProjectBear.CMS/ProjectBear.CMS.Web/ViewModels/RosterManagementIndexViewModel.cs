using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBear.CMS.ViewModels
{
    public class RosterManagementIndexViewModel
    {
        public List<RosterManagementViewModel> RosterList { get; set; }
        public List<RosterTemplateManagementViewModel> TemplateList { get; set; }
        public RosterTemplateManagementViewModel SelectedTemplate { get; set; }
    }
}