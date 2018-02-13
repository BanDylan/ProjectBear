using Serenity.Navigation;
using Administration = ProjectBear.CMS.Administration.Pages;

[assembly: NavigationLink(2000, "Rosters", typeof(ProjectBear.CMS.Modules.Content.RosterManagement.RosterManagementController))]

[assembly: NavigationLink(3000, "Roster Templates", typeof(ProjectBear.CMS.Modules.Content.RosterTemplateManagement.RosterTemplateManagementController))]

[assembly: NavigationLink(4000, "Players", typeof(ProjectBear.CMS.Modules.Content.PlayerManagement.PlayerManagementController))]

