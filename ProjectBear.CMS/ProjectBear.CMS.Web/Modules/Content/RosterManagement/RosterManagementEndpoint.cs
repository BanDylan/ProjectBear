using Serenity.Data;
using Serenity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectBear.CMS.Modules.Content.RosterManagement
{
    [RoutePrefix("Services/Content/RosterManagement"), Route("{action}")]
    [ConnectionKey("Default"), ServiceAuthorize("Administration")]
    public class RosterManagementEndpoint : ServiceEndpoint
    {

    }
}