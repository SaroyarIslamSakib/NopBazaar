using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.FaqManager.Public.Models;

public record ProductFaqModel : BaseNopModel
{
    public string GroupName { get; set; } = string.Empty;

    public int FaqCount { get; set; }

    public bool ShowFaqCount { get; set; }

    public IList<ProductFaqItemModel> Items { get; set; } = new List<ProductFaqItemModel>();
}
