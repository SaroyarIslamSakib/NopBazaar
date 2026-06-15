using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqGroupSearchModel : BaseSearchModel
{
    #region Ctor

    public FaqGroupSearchModel()
    {
        AvailableProducts = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqGroups.Fields.Product")]
    public int SearchProductId { get; set; }

    public string SearchProductName { get; set; }

    public IList<SelectListItem> AvailableProducts { get; set; }

    #endregion
}