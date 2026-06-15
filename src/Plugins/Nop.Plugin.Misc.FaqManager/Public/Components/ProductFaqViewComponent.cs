using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.FaqManager.Public.Models;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.FaqManager.Public.Components;

public class ProductFaqViewComponent : NopViewComponent
{
    #region Fields

    private readonly FaqManagerSettings _faqManagerSettings;
    private readonly FaqService _faqService;
    private readonly ILocalizationService _localizationService;

    #endregion

    #region Ctor

    public ProductFaqViewComponent(FaqManagerSettings faqManagerSettings,
        FaqService faqService,
        ILocalizationService localizationService)
    {
        _faqManagerSettings = faqManagerSettings;
        _faqService = faqService;
        _localizationService = localizationService;
    }

    #endregion

    #region Methods

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        if (additionalData is not ProductDetailsModel productDetails)
            return Content(string.Empty);

        var productId = productDetails.Id;
        if (productId == 0)
            return Content(string.Empty);

        var faqGroup = await _faqService.GetFaqGroupByProductIdAsync(productId);
        if (faqGroup == null)
            return Content(string.Empty);

        var faqItems = await _faqService.GetFaqItemsByGroupIdAsync(faqGroup.Id);

        var model = new ProductFaqModel
        {
            GroupName = await _localizationService.GetLocalizedAsync(faqGroup, g => g.Name),
            ShowFaqCount = _faqManagerSettings.ShowFaqCount,
            FaqCount = faqItems.Count
        };

        foreach (var item in faqItems)
        {
            model.Items.Add(new ProductFaqItemModel
            {
                Id = item.Id,
                Question = await _localizationService.GetLocalizedAsync(item, i => i.Question),
                Answer = await _localizationService.GetLocalizedAsync(item, i => i.Answer),
                DisplayOrder = item.DisplayOrder
            });
        }

        if (!model.Items.Any())
            return Content(string.Empty);

        return await ViewAsync("~/Plugins/Misc.FaqManager/Public/Views/Components/ProductFaq/Default.cshtml", model);
    }

    #endregion
}