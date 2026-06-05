using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.ManufacturerShowcase.Models;

public record ConfigurationModel : BaseNopModel
{
    #region Properties

    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.ManufacturerShowcase.DisplayNumber")]
    public int DisplayNumber { get; set; }
    public bool DisplayNumber_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.ManufacturerShowcase.ShowImages")]
    public bool ShowImages { get; set; }
    public bool ShowImages_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.ManufacturerShowcase.ShowNames")]
    public bool ShowNames { get; set; }
    public bool ShowNames_OverrideForStore { get; set; }

    #endregion
}