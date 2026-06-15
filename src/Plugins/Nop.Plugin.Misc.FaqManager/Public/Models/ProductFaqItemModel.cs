using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.FaqManager.Public.Models;

public record ProductFaqItemModel : BaseNopEntityModel
{
    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}