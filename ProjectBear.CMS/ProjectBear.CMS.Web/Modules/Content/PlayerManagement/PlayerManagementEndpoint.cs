using Serenity.Data;
using Serenity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectBear.CMS.Modules.Content.PlayerManagement
{
    [RoutePrefix("Services/Content/PlayerManagement"), Route("{action}")]
    [ConnectionKey("Default"), ServiceAuthorize("Administration")]
    public class PlayerManagementEndpoint : ServiceEndpoint
    {

    }
}