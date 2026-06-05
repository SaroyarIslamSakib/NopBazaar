using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.ManufacturerShowcase.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.ManufacturerShowcase.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class WidgetManufacturerShowcaseController : BasePluginController
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public WidgetManufacturerShowcaseController(
        ILocalizationService localizationService,
        INotificationService notificationService,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure()
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<ManufacturerShowcaseSettings>(storeScope);

        var model = new ConfigurationModel
        {
            DisplayNumber = settings.DisplayNumber,
            ShowImages = settings.ShowImages,
            ShowNames = settings.ShowNames,
            ActiveStoreScopeConfiguration = storeScope
        };

        if (storeScope > 0)
        {
            model.DisplayNumber_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.DisplayNumber, storeScope);
            model.ShowImages_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.ShowImages, storeScope);
            model.ShowNames_OverrideForStore = await _settingService.SettingExistsAsync(settings, x => x.ShowNames, storeScope);
        }

        return View("~/Plugins/Widgets.ManufacturerShowcase/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<ManufacturerShowcaseSettings>(storeScope);

        settings.DisplayNumber = model.DisplayNumber;
        settings.ShowImages = model.ShowImages;
        settings.ShowNames = model.ShowNames;

        await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.DisplayNumber, model.DisplayNumber_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.ShowImages, model.ShowImages_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(settings, x => x.ShowNames, model.ShowNames_OverrideForStore, storeScope, false);

        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion
}