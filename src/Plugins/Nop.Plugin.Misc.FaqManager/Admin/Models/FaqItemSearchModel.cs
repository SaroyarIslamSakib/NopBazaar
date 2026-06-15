using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqItemSearchModel : BaseSearchModel
{
    #region Properties

    public int FaqGroupId { get; set; }

    #endregion
}