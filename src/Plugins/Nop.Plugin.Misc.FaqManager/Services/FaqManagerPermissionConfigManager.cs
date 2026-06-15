using Nop.Core.Domain.Customers;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.FaqManager.Services;

public class FaqManagerPermissionConfigManager : IPermissionConfigManager
{
    public IList<PermissionConfig> AllConfigs => new List<PermissionConfig>
    {
        new("Admin area. FAQ Manager. View",
            FaqManagerDefaults.Permissions.FAQ_VIEW,
            nameof(StandardPermission.ContentManagement),
            NopCustomerDefaults.AdministratorsRoleName),
        new("Admin area. FAQ Manager. Create, edit, delete",
            FaqManagerDefaults.Permissions.FAQ_MANAGE,
            nameof(StandardPermission.ContentManagement),
            NopCustomerDefaults.AdministratorsRoleName)
    };
}