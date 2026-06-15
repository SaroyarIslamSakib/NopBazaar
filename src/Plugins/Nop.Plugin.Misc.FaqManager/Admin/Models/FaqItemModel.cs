using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FaqManager.Admin.Models;

public record FaqItemModel : BaseNopEntityModel, ILocalizedModel<FaqItemModel.FaqItemLocalizedModel>
{
    #region Ctor

    public FaqItemModel()
    {
        Locales = new List<FaqItemLocalizedModel>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqItems.Fields.Question")]
    public string Question { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqItems.Fields.Answer")]
    public string Answer { get; set; }

    public int FaqGroupId { get; set; }

    public string? GroupName { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqItems.Fields.Published")]
    public bool Published { get; set; }

    [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqItems.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<FaqItemLocalizedModel> Locales { get; set; }

    #endregion

    #region Nested class

    public class FaqItemLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqItems.Fields.Question")]
        public string Question { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FaqManager.FaqItems.Fields.Answer")]
        public string Answer { get; set; }
    }

    #endregion
}