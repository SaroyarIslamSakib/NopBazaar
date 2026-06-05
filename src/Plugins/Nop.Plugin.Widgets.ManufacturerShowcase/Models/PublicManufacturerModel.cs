using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.ManufacturerShowcase.Models;

public record PublicManufacturerModel : BaseNopModel
{
    #region Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string SeName { get; set; }
    public string PictureUrl { get; set; }
    public bool LazyLoading { get; set; }

    #endregion
}