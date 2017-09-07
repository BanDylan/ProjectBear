using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBear.CMS.ViewModels
{
    public class RosterManagementViewModel
    {
        public Guid RosterId { get; set; }

        public DateTime Date { get; set; }

        public bool IsPublished { get; set; }
    }
}