using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqContentModel : BaseNopModel
{
    #region Ctor

    public FaqContentModel()
    {
        FaqGroupSearch = new FaqGroupSearchModel();
    }

    #endregion

    #region Properties

    public FaqGroupSearchModel FaqGroupSearch { get; set; }

    #endregion
}