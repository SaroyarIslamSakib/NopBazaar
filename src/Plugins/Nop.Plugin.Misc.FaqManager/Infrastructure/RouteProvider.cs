using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace Nop.Plugin.Misc.FaqManager.Infrastructure;

public class RouteProvider : BaseRouteProvider, IRouteProvider
{
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.ConfigurationRouteName,
            pattern: "Admin/FaqManager/Configure",
            defaults: new { controller = "FaqManagerAdmin", action = "Configure", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupsRouteName,
            pattern: "Admin/FaqGroup",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqGroups", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupListRouteName,
            pattern: "Admin/FaqGroup/List",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqGroupList", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupCreateRouteName,
            pattern: "Admin/FaqGroup/Create",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqGroupCreate", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupEditRouteName,
            pattern: "Admin/FaqGroup/Edit/{id:min(0)}",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqGroupEdit", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqGroupDeleteRouteName,
            pattern: "Admin/FaqGroup/Delete",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqGroupDelete", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqItemListRouteName,
            pattern: "Admin/FaqItem/List/{faqGroupId:min(0)}",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqItemList", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqItemCreateRouteName,
            pattern: "Admin/FaqItem/Create/{faqGroupId:min(0)}",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqItemCreate", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqItemEditRouteName,
            pattern: "Admin/FaqItem/Edit/{id:min(0)}",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqItemEdit", area = AreaNames.ADMIN });

        endpointRouteBuilder.MapControllerRoute(name: FaqManagerDefaults.Routes.Admin.FaqItemDeleteRouteName,
            pattern: "Admin/FaqItem/Delete",
            defaults: new { controller = "FaqManagerAdmin", action = "FaqItemDelete", area = AreaNames.ADMIN });
    }

    public int Priority => 0;
}