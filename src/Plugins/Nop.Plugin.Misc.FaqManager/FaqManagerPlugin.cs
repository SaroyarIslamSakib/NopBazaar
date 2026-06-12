using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Localization;
using Nop.Plugin.Misc.FaqManager.Public.Components;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.FaqManager;

public class FaqManagerPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
{
    #region Fields

    private readonly INopUrlHelper _nopUrlHelper;
    private readonly ISettingService _settingService;
    private readonly FaqInstallService _faqInstallService;
    private readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public FaqManagerPlugin(INopUrlHelper nopUrlHelper,
        ISettingService settingService,
        FaqInstallService faqInstallService,
        WidgetSettings widgetSettings)
    {
        _nopUrlHelper = nopUrlHelper;
        _settingService = settingService;
        _faqInstallService = faqInstallService;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region Methods

    public override string GetConfigurationPageUrl()
    {
        return _nopUrlHelper.RouteUrl(FaqManagerDefaults.Routes.Admin.ConfigurationRouteName);
    }

    public override async Task InstallAsync()
    {
        await _faqInstallService.InstallRequiredDataAsync();

        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(FaqManagerDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(FaqManagerDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _faqInstallService.UninstallRequiredDataAsync();

        if (_widgetSettings.ActiveWidgetSystemNames.Contains(FaqManagerDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(FaqManagerDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await base.UninstallAsync();
    }

    #region IWidgetPlugin

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ProductFaqViewComponent);
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>([PublicWidgetZones.ProductDetailsBottom]);
    }

    public bool HideInWidgetList => true;

    #endregion

    #endregion
}