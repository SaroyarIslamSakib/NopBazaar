using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.FaqManager.Admin.Factories;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.FaqManager.Admin.Controllers;

[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
[ValidateIpAddress]
[AuthorizeAdmin]
[SaveSelectedTab]
public class FaqManagerAdminController : BasePluginController
{
    #region Fields

    private readonly FaqManagerSettings _faqManagerSettings;
    private readonly FaqModelFactory _faqModelFactory;
    private readonly FaqService _faqService;
    private readonly ICustomerActivityService _customerActivityService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public FaqManagerAdminController(FaqManagerSettings faqManagerSettings,
        FaqModelFactory faqModelFactory,
        FaqService faqService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _faqManagerSettings = faqManagerSettings;
        _faqModelFactory = faqModelFactory;
        _faqService = faqService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Configure

    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public async Task<IActionResult> Configure()
    {
        var model = await _faqModelFactory.PrepareConfigurationModelAsync();

        return View("~/Plugins/Misc.FaqManager/Admin/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
        {
            model = await _faqModelFactory.PrepareConfigurationModelAsync(model);

            return View("~/Plugins/Misc.FaqManager/Admin/Views/Configure.cshtml", model);
        }

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var faqManagerSettings = await _settingService.LoadSettingAsync<FaqManagerSettings>(storeScope);
        faqManagerSettings = model.ToSettings(faqManagerSettings);

        await _settingService.SaveSettingOverridablePerStoreAsync(faqManagerSettings, x => x.ShowFaqCount, model.ShowFaqCount_OverrideForStore, storeScope, false);

        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

        return RedirectToAction(nameof(Configure));
    }

    #endregion

    #region FaqGroups

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_VIEW)]
    public async Task<IActionResult> FaqGroups()
    {
        var model = await _faqModelFactory.PrepareFaqContentModelAsync(new FaqContentModel());

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroups.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_VIEW)]
    public async Task<IActionResult> FaqGroupList(FaqGroupSearchModel searchModel)
    {
        var model = await _faqModelFactory.PrepareFaqGroupListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqGroupCreate()
    {
        var model = await _faqModelFactory.PrepareFaqGroupModelAsync(new FaqGroupModel(), null);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroupCreate.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqGroupCreate(FaqGroupModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var faqGroup = model.ToEntity<FaqGroup>();
            await _faqService.InsertFaqGroupAsync(faqGroup);

            await _customerActivityService.InsertActivityAsync(FaqManagerDefaults.ActivityLogTypeSystemNames.AddNewFaqGroup,
                string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.ActivityLog.AddNewFaqGroup"), faqGroup.Id), faqGroup);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqGroups.Added"));

            if (!continueEditing)
                return RedirectToAction(nameof(FaqGroups));

            return RedirectToAction(nameof(FaqGroupEdit), new { id = faqGroup.Id });
        }

        model = await _faqModelFactory.PrepareFaqGroupModelAsync(model, null, true);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroupCreate.cshtml", model);
    }

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_VIEW)]
    public async Task<IActionResult> FaqGroupEdit(int id)
    {
        var faqGroup = await _faqService.GetFaqGroupByIdAsync(id);
        if (faqGroup == null)
            return RedirectToAction(nameof(FaqGroups));

        var model = await _faqModelFactory.PrepareFaqGroupModelAsync(null, faqGroup);
        model.FaqItemSearchModel = await _faqModelFactory.PrepareFaqItemSearchModelAsync(model.FaqItemSearchModel, faqGroup);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroupEdit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqGroupEdit(FaqGroupModel model, bool continueEditing)
    {
        var faqGroup = await _faqService.GetFaqGroupByIdAsync(model.Id);
        if (faqGroup == null)
            return RedirectToAction(nameof(FaqGroups));

        if (ModelState.IsValid)
        {
            faqGroup = model.ToEntity(faqGroup);
            await _faqService.UpdateFaqGroupAsync(faqGroup);

            await _customerActivityService.InsertActivityAsync(FaqManagerDefaults.ActivityLogTypeSystemNames.EditFaqGroup,
                string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.ActivityLog.EditFaqGroup"), faqGroup.Id), faqGroup);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqGroups.Updated"));

            if (!continueEditing)
                return RedirectToAction(nameof(FaqGroups));

            return RedirectToAction(nameof(FaqGroupEdit), new { id = faqGroup.Id });
        }

        model = await _faqModelFactory.PrepareFaqGroupModelAsync(model, faqGroup, true);
        model.FaqItemSearchModel = await _faqModelFactory.PrepareFaqItemSearchModelAsync(model.FaqItemSearchModel, faqGroup);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqGroupEdit.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqGroupDelete(int id)
    {
        var faqGroup = await _faqService.GetFaqGroupByIdAsync(id);
        if (faqGroup == null)
            return RedirectToAction(nameof(FaqGroups));

        await _faqService.DeleteFaqGroupAsync(faqGroup);

        await _customerActivityService.InsertActivityAsync(FaqManagerDefaults.ActivityLogTypeSystemNames.DeleteFaqGroup,
            string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.ActivityLog.DeleteFaqGroup"), faqGroup.Id), faqGroup);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqGroups.Deleted"));

        return RedirectToAction(nameof(FaqGroups));
    }

    #endregion

    #region FaqItems

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_VIEW)]
    public async Task<IActionResult> FaqItemList(FaqItemSearchModel searchModel)
    {
        var model = await _faqModelFactory.PrepareFaqItemListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqItemCreate(int faqGroupId)
    {
        var faqGroup = await _faqService.GetFaqGroupByIdAsync(faqGroupId);
        if (faqGroup == null)
            return RedirectToAction(nameof(FaqGroups));

        var model = await _faqModelFactory.PrepareFaqItemModelAsync(new FaqItemModel(), null);
        model.FaqGroupId = faqGroupId;
        model.GroupName = await _localizationService.GetLocalizedAsync(faqGroup, g => g.Name);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItemCreate.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqItemCreate(FaqItemModel model, bool continueEditing)
    {
        var faqGroup = await _faqService.GetFaqGroupByIdAsync(model.FaqGroupId);
        if (faqGroup == null)
            return RedirectToAction(nameof(FaqGroups));

        if (ModelState.IsValid)
        {
            var faqItem = model.ToEntity<FaqItem>();
            faqItem.FaqGroupId = model.FaqGroupId;
            await _faqService.InsertFaqItemAsync(faqItem);

            await _customerActivityService.InsertActivityAsync(FaqManagerDefaults.ActivityLogTypeSystemNames.AddNewFaqItem,
                string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.ActivityLog.AddNewFaqItem"), faqItem.Id), faqItem);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqItems.Added"));

            if (!continueEditing)
                return RedirectToAction(nameof(FaqGroupEdit), new { id = faqGroup.Id });

            return RedirectToAction(nameof(FaqItemEdit), new { id = faqItem.Id });
        }

        model = await _faqModelFactory.PrepareFaqItemModelAsync(model, null, true);
        model.GroupName = await _localizationService.GetLocalizedAsync(faqGroup, g => g.Name);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItemCreate.cshtml", model);
    }

    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_VIEW)]
    public async Task<IActionResult> FaqItemEdit(int id)
    {
        var faqItem = await _faqService.GetFaqItemByIdAsync(id);
        if (faqItem == null)
            return RedirectToAction(nameof(FaqGroups));

        var model = await _faqModelFactory.PrepareFaqItemModelAsync(null, faqItem);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItemEdit.cshtml", model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqItemEdit(FaqItemModel model, bool continueEditing)
    {
        var faqItem = await _faqService.GetFaqItemByIdAsync(model.Id);
        if (faqItem == null)
            return RedirectToAction(nameof(FaqGroups));

        if (ModelState.IsValid)
        {
            faqItem = model.ToEntity(faqItem);
            await _faqService.UpdateFaqItemAsync(faqItem);

            await _customerActivityService.InsertActivityAsync(FaqManagerDefaults.ActivityLogTypeSystemNames.EditFaqItem,
                string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.ActivityLog.EditFaqItem"), faqItem.Id), faqItem);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqItems.Updated"));

            if (!continueEditing)
                return RedirectToAction(nameof(FaqGroupEdit), new { id = faqItem.FaqGroupId });

            return RedirectToAction(nameof(FaqItemEdit), new { id = faqItem.Id });
        }

        model = await _faqModelFactory.PrepareFaqItemModelAsync(model, faqItem, true);

        return View("~/Plugins/Misc.FaqManager/Admin/Views/FaqItemEdit.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(FaqManagerDefaults.Permissions.FAQ_MANAGE)]
    public async Task<IActionResult> FaqItemDelete(int id)
    {
        var faqItem = await _faqService.GetFaqItemByIdAsync(id);
        if (faqItem == null)
            return RedirectToAction(nameof(FaqGroups));

        var faqGroupId = faqItem.FaqGroupId;

        await _faqService.DeleteFaqItemAsync(faqItem);

        await _customerActivityService.InsertActivityAsync(FaqManagerDefaults.ActivityLogTypeSystemNames.DeleteFaqItem,
            string.Format(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.ActivityLog.DeleteFaqItem"), faqItem.Id), faqItem);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.FaqManager.FaqItems.Deleted"));

        return RedirectToAction(nameof(FaqGroupEdit), new { id = faqGroupId });
    }

    #endregion
}