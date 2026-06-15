using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.FaqManager.Services.Events;

public class AdminMenuCreatedEventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly INopUrlHelper _nopUrlHelper;
    private readonly IWidgetPluginManager _pluginManager;

    #endregion

    #region Ctor

    public AdminMenuCreatedEventConsumer(ILocalizationService localizationService,
        INopUrlHelper nopUrlHelper,
        IWidgetPluginManager pluginManager)
    {
        _localizationService = localizationService;
        _nopUrlHelper = nopUrlHelper;
        _pluginManager = pluginManager;
    }

    #endregion

    #region Methods

    public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        var plugin = await _pluginManager.LoadPluginBySystemNameAsync(FaqManagerDefaults.SystemName);

        if (plugin == null || !_pluginManager.IsPluginActive(plugin))
            return;

        //insert after "News items" if News plugin is active, otherwise after "Message templates"
        var insertAfterSystemName = eventMessage.RootMenuItem.ContainsSystemName("News items")
            ? "News items"
            : "Message templates";

        eventMessage.RootMenuItem.InsertAfter(insertAfterSystemName, new()
        {
            SystemName = FaqManagerDefaults.FaqGroupsMenuSystemName,
            Title = await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqGroups"),
            IconClass = "far fa-dot-circle",
            Url = _nopUrlHelper.RouteUrl(FaqManagerDefaults.Routes.Admin.FaqGroupsRouteName),
            PermissionNames = new List<string> { FaqManagerDefaults.Permissions.FAQ_VIEW }
        });
    }

    #endregion
}