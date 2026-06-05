using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.ManufacturerShowcase;

public class ManufacturerShowcaseSettings : ISettings
{
    #region Properties

    public int DisplayNumber { get; set; }
    public bool ShowImages { get; set; }
    public bool ShowNames { get; set; }

    #endregion
}