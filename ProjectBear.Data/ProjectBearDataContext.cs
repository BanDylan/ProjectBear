using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBear.Data
{
    public class ProjectBearDataContext : IdentityDbContext<ApplicationUser>
    {
        public ProjectBearDataContext()
            : base("Default", throwIfV1Schema: false)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProjectBearDataContext, ProjectBear.Migrations.Configuration>("Default"));
        }

        public static ProjectBearDataContext Create()
        {
            return new ProjectBearDataContext();
        }
    }
}
