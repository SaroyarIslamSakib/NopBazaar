using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ManufacturerShowcase.Models;

public record PublicInfoModel : BaseNopModel
{
    #region Properties

    public bool ShowImages { get; set; }
    public bool ShowNames { get; set; }
    public List<PublicManufacturerModel> Manufacturers { get; set; } = new();

    #endregion
}