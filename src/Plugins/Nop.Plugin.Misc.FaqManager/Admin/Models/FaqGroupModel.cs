using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqGroupModel : BaseNopEntityModel, ILocalizedModel<FaqGroupModel.FaqGroupLocalizedModel>
{
    #region Ctor

    public FaqGroupModel()
    {
        Locales = new List<FaqGroupLocalizedModel>();
        AvailableProducts = new List<SelectListItem>();
        FaqItemSearchModel = new FaqItemSearchModel();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqGroups.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqGroups.Fields.Product")]
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public IList<SelectListItem> AvailableProducts { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqGroups.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqGroups.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<FaqGroupLocalizedModel> Locales { get; set; }

    public FaqItemSearchModel FaqItemSearchModel { get; set; }

    #endregion

    #region Nested class

    public class FaqGroupLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqGroups.Fields.Name")]
        public string Name { get; set; }
    }

    #endregion
}