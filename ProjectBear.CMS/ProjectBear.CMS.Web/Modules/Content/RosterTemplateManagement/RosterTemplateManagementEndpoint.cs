using Serenity.Data;
using Serenity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectBear.CMS.Modules.Content.RosterTemplateManagement
{
    [RoutePrefix("Services/Content/RosterTemplateManagement"), Route("{action}")]
    [ConnectionKey("Default"), ServiceAuthorize("Administration")]
    public class RosterTemplateManagementEndpoint : ServiceEndpoint
    {

    }
}