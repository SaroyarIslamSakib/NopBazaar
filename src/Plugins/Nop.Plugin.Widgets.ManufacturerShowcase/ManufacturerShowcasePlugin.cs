using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.ManufacturerShowcase.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.ManufacturerShowcase;

public class ManufacturerShowcasePlugin : BasePlugin, IWidgetPlugin
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly ISettingService _settingService;
    protected readonly IWebHelper _webHelper;
    protected readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public ManufacturerShowcasePlugin(
        ILocalizationService localizationService,
        ISettingService settingService,
        IWebHelper webHelper,
        WidgetSettings widgetSettings)
    {
        _localizationService = localizationService;
        _settingService = settingService;
        _webHelper = webHelper;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region Methods

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBottom });
    }

    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/WidgetManufacturerShowcase/Configure";
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(WidgetManufacturerShowcaseViewComponent);
    }

    public override async Task InstallAsync()
    {
        var settings = new ManufacturerShowcaseSettings
        {
            DisplayNumber = 8,
            ShowImages = true,
            ShowNames = true
        };
        await _settingService.SaveSettingAsync(settings);

        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(PluginDescriptor.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(PluginDescriptor.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Widgets.ManufacturerShowcase.DisplayNumber"] = "Number of manufacturers",
            ["Plugins.Widgets.ManufacturerShowcase.DisplayNumber.Hint"] = "Set maximum number of manufacturers to display.",
            ["Plugins.Widgets.ManufacturerShowcase.DisplayNumber.Required"] = "Display number is required",
            ["Plugins.Widgets.ManufacturerShowcase.ShowImages"] = "Show images",
            ["Plugins.Widgets.ManufacturerShowcase.ShowImages.Hint"] = "Check to display manufacturer logos.",
            ["Plugins.Widgets.ManufacturerShowcase.ShowNames"] = "Show names",
            ["Plugins.Widgets.ManufacturerShowcase.ShowNames.Hint"] = "Check to display manufacturer names.",
        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<ManufacturerShowcaseSettings>();

        if (_widgetSettings.ActiveWidgetSystemNames.Contains(PluginDescriptor.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(PluginDescriptor.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.ManufacturerShowcase");

        await base.UninstallAsync();
    }

    #endregion

    #region Properties

    public bool HideInWidgetList => false;

    #endregion
}