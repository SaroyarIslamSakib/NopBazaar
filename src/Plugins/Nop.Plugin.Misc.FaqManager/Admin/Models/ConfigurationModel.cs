using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record ConfigurationModel : BaseNopModel, ISettingsModel
{
    #region Properties

    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.Configuration.Fields.ShowFaqCount")]
    public bool ShowFaqCount { get; set; }

    public bool ShowFaqCount_OverrideForStore { get; set; }

    #endregion
}