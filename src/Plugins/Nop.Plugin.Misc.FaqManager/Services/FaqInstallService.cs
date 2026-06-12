using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Logging;
using Nop.Core.Domain.Security;
using Nop.Data;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.FaqManager.Services;

public class FaqInstallService
{
    #region Fields

    private readonly ICustomerService _customerService;
    private readonly ILocalizationService _localizationService;
    private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
    private readonly IRepository<PermissionRecord> _permissionRepository;
    private readonly IRepository<PermissionRecordCustomerRoleMapping> _permissionMappingRepository;
    private readonly ISettingService _settingService;

    #endregion

    #region Ctor

    public FaqInstallService(ICustomerService customerService,
        ILocalizationService localizationService,
        IRepository<ActivityLogType> activityLogTypeRepository,
        IRepository<PermissionRecord> permissionRepository,
        IRepository<PermissionRecordCustomerRoleMapping> permissionMappingRepository,
        ISettingService settingService)
    {
        _customerService = customerService;
        _localizationService = localizationService;
        _activityLogTypeRepository = activityLogTypeRepository;
        _permissionRepository = permissionRepository;
        _permissionMappingRepository = permissionMappingRepository;
        _settingService = settingService;
    }

    #endregion

    #region Utilities

    private async Task InsertSettingsAsync()
    {
        var faqSettings = await _settingService.LoadSettingAsync<FaqManagerSettings>();

        if (!await _settingService.SettingExistsAsync(faqSettings, s => s.ShowFaqCount))
        {
            await _settingService.SaveSettingAsync(new FaqManagerSettings
            {
                ShowFaqCount = false
            });
        }
    }

    private async Task InsertLocalesAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            //Activity log
            ["Plugins.Misc.FaqManager.ActivityLog.AddNewFaqGroup"] = "Added a new FAQ group (ID = {0})",
            ["Plugins.Misc.FaqManager.ActivityLog.EditFaqGroup"] = "Edited a FAQ group (ID = {0})",
            ["Plugins.Misc.FaqManager.ActivityLog.DeleteFaqGroup"] = "Deleted a FAQ group (ID = {0})",
            ["Plugins.Misc.FaqManager.ActivityLog.AddNewFaqItem"] = "Added a new FAQ item (ID = {0})",
            ["Plugins.Misc.FaqManager.ActivityLog.EditFaqItem"] = "Edited a FAQ item (ID = {0})",
            ["Plugins.Misc.FaqManager.ActivityLog.DeleteFaqItem"] = "Deleted a FAQ item (ID = {0})",

            //Admin - Configuration
            ["Plugins.Misc.FaqManager.Configuration.Fields.ShowFaqCount"] = "Show FAQ count",
            ["Plugins.Misc.FaqManager.Configuration.Fields.ShowFaqCount.Hint"] = "Check to show the number of FAQ items next to the section heading on the product page.",

            //Admin - FAQ Groups
            ["Plugins.Misc.FaqManager.FaqGroups"] = "FAQ Groups",
            ["Plugins.Misc.FaqManager.FaqGroups.Added"] = "The new FAQ group has been added successfully.",
            ["Plugins.Misc.FaqManager.FaqGroups.AddNew"] = "Add a new FAQ group",
            ["Plugins.Misc.FaqManager.FaqGroups.BackToList"] = "back to FAQ group list",
            ["Plugins.Misc.FaqManager.FaqGroups.Deleted"] = "The FAQ group has been deleted successfully.",
            ["Plugins.Misc.FaqManager.FaqGroups.EditFaqGroupDetails"] = "Edit FAQ group details",
            ["Plugins.Misc.FaqManager.FaqGroups.Updated"] = "The FAQ group has been updated successfully.",

            //Admin - FAQ Group Fields
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.Name"] = "Name",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.Name.Hint"] = "The name of the FAQ group.",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.Name.Required"] = "Name is required.",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.Product"] = "Product",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.Product.Hint"] = "Choose the product this FAQ group belongs to.",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.Published"] = "Published",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.Published.Hint"] = "Determines whether the FAQ group is visible on the product page.",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.DisplayOrder"] = "Display order",
            ["Plugins.Misc.FaqManager.FaqGroups.Fields.DisplayOrder.Hint"] = "The display order of this FAQ group. Lower values are displayed first.",

            //Admin - FAQ Group List
            ["Plugins.Misc.FaqManager.FaqGroups.List.SearchProduct"] = "Product",
            ["Plugins.Misc.FaqManager.FaqGroups.List.SearchProduct.Hint"] = "Search by product name.",

            //Admin - FAQ Items
            ["Plugins.Misc.FaqManager.FaqItems"] = "FAQ Items",
            ["Plugins.Misc.FaqManager.FaqItems.Added"] = "The new FAQ item has been added successfully.",
            ["Plugins.Misc.FaqManager.FaqItems.AddNew"] = "Add a new FAQ item",
            ["Plugins.Misc.FaqManager.FaqItems.Deleted"] = "The FAQ item has been deleted successfully.",
            ["Plugins.Misc.FaqManager.FaqItems.EditFaqItemDetails"] = "Edit FAQ item details",
            ["Plugins.Misc.FaqManager.FaqItems.Updated"] = "The FAQ item has been updated successfully.",

            //Admin - FAQ Item Fields
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Question"] = "Question",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Question.Hint"] = "The question text.",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Question.Required"] = "Question is required.",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Answer"] = "Answer",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Answer.Hint"] = "The answer text.",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Answer.Required"] = "Answer is required.",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Published"] = "Published",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.Published.Hint"] = "Determines whether this FAQ item is visible on the product page.",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.DisplayOrder"] = "Display order",
            ["Plugins.Misc.FaqManager.FaqItems.Fields.DisplayOrder.Hint"] = "The display order of this FAQ item. Lower values are displayed first.",

            //Public
            ["Plugins.Misc.FaqManager.Public.FaqSectionTitle"] = "Frequently Asked Questions",
            ["Plugins.Misc.FaqManager.Public.NoFaqs"] = "No FAQs available.",

            //Permissions
            ["Security.Permission.FaqManager.View"] = "Admin area. FAQ Manager. View",
            ["Security.Permission.FaqManager.Manage"] = "Admin area. FAQ Manager. Create, edit, delete"
        });
    }

    private async Task InsertActivityLogTypesAsync()
    {
        var activityLogTypes = new List<(string SystemKeyword, string Name, bool Enabled)>
        {
            (FaqManagerDefaults.ActivityLogTypeSystemNames.AddNewFaqGroup, "Add a new FAQ group", true),
            (FaqManagerDefaults.ActivityLogTypeSystemNames.EditFaqGroup, "Edit a FAQ group", true),
            (FaqManagerDefaults.ActivityLogTypeSystemNames.DeleteFaqGroup, "Delete a FAQ group", true),
            (FaqManagerDefaults.ActivityLogTypeSystemNames.AddNewFaqItem, "Add a new FAQ item", true),
            (FaqManagerDefaults.ActivityLogTypeSystemNames.EditFaqItem, "Edit a FAQ item", true),
            (FaqManagerDefaults.ActivityLogTypeSystemNames.DeleteFaqItem, "Delete a FAQ item", true),
        };

        foreach (var (systemKeyword, name, enabled) in activityLogTypes)
        {
            if (!await _activityLogTypeRepository.Table.AnyAsync(alt => alt.SystemKeyword == systemKeyword))
            {
                await _activityLogTypeRepository.InsertAsync(new ActivityLogType
                {
                    SystemKeyword = systemKeyword,
                    Name = name,
                    Enabled = enabled
                });
            }
        }
    }

    private async Task InsertPermissionsAsync()
    {
        var adminRole = await _customerService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.AdministratorsRoleName);

        var permissions = new List<(string SystemName, string Name)>
        {
            (FaqManagerDefaults.Permissions.FAQ_VIEW, "Admin area. FAQ Manager. View"),
            (FaqManagerDefaults.Permissions.FAQ_MANAGE, "Admin area. FAQ Manager. Create, edit, delete")
        };

        foreach (var (systemName, name) in permissions)
        {
            if (!await _permissionRepository.Table.AnyAsync(pr => pr.SystemName == systemName))
            {
                var permissionRecord = new PermissionRecord
                {
                    SystemName = systemName,
                    Name = name,
                    Category = "FAQ Manager"
                };

                await _permissionRepository.InsertAsync(permissionRecord);

                if (adminRole is not null)
                {
                    await _permissionMappingRepository.InsertAsync(new PermissionRecordCustomerRoleMapping
                    {
                        CustomerRoleId = adminRole.Id,
                        PermissionRecordId = permissionRecord.Id
                    });
                }
            }
        }
    }

    #endregion

    #region Methods

    public async Task InstallRequiredDataAsync()
    {
        await InsertSettingsAsync();
        await InsertLocalesAsync();
        await InsertActivityLogTypesAsync();
        await InsertPermissionsAsync();
    }

    public async Task UninstallRequiredDataAsync()
    {
        //settings
        await _settingService.DeleteSettingAsync<FaqManagerSettings>();

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.FaqManager.");
        await _localizationService.DeleteLocaleResourcesAsync("Security.Permission.FaqManager.");

        //activity log types
        await _activityLogTypeRepository.DeleteAsync(at => at.SystemKeyword == FaqManagerDefaults.ActivityLogTypeSystemNames.AddNewFaqGroup);
        await _activityLogTypeRepository.DeleteAsync(at => at.SystemKeyword == FaqManagerDefaults.ActivityLogTypeSystemNames.EditFaqGroup);
        await _activityLogTypeRepository.DeleteAsync(at => at.SystemKeyword == FaqManagerDefaults.ActivityLogTypeSystemNames.DeleteFaqGroup);
        await _activityLogTypeRepository.DeleteAsync(at => at.SystemKeyword == FaqManagerDefaults.ActivityLogTypeSystemNames.AddNewFaqItem);
        await _activityLogTypeRepository.DeleteAsync(at => at.SystemKeyword == FaqManagerDefaults.ActivityLogTypeSystemNames.EditFaqItem);
        await _activityLogTypeRepository.DeleteAsync(at => at.SystemKeyword == FaqManagerDefaults.ActivityLogTypeSystemNames.DeleteFaqItem);

        //permissions
        await _permissionRepository.DeleteAsync(record => record.SystemName == FaqManagerDefaults.Permissions.FAQ_VIEW
            || record.SystemName == FaqManagerDefaults.Permissions.FAQ_MANAGE);
    }

    #endregion
}