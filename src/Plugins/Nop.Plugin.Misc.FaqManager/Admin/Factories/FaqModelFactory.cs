using Nop.Core;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Plugin.Misc.FaqManager.Services;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.FaqManager.Admin.Factories;

public class FaqModelFactory
{
    #region Fields

    private readonly FaqService _faqService;
    private readonly ILocalizationService _localizationService;
    private readonly IProductService _productService;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public FaqModelFactory(FaqService faqService,
        ILocalizationService localizationService,
        IProductService productService,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _faqService = faqService;
        _localizationService = localizationService;
        _productService = productService;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    #region Configuration

    public virtual async Task<ConfigurationModel> PrepareConfigurationModelAsync(ConfigurationModel model = null)
    {
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var faqManagerSettings = await _settingService.LoadSettingAsync<FaqManagerSettings>(storeId);

        model ??= faqManagerSettings.ToSettingsModel<ConfigurationModel>();

        model.ActiveStoreScopeConfiguration = storeId;

        if (storeId > 0)
            model.ShowFaqCount_OverrideForStore = await _settingService.SettingExistsAsync(faqManagerSettings, x => x.ShowFaqCount, storeId);

        return model;
    }

    #endregion

    #region FaqGroup

    public virtual async Task<FaqContentModel> PrepareFaqContentModelAsync(FaqContentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        await PrepareFaqGroupSearchModelAsync(model.FaqGroupSearch);

        return model;
    }

    public virtual async Task<FaqGroupSearchModel> PrepareFaqGroupSearchModelAsync(FaqGroupSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.SetGridPageSize();

        return searchModel;
    }

    public virtual async Task<FaqGroupListModel> PrepareFaqGroupListModelAsync(FaqGroupSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var groups = await _faqService.GetAllFaqGroupsAsync(
            productId: searchModel.SearchProductId,
            showHidden: true,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = await new FaqGroupListModel().PrepareToGridAsync(searchModel, groups, () =>
        {
            return groups.SelectAwait(async faqGroup =>
            {
                var faqGroupModel = faqGroup.ToModel<FaqGroupModel>();

                faqGroupModel.Name = await _localizationService.GetLocalizedAsync(faqGroup, f => f.Name);
                faqGroupModel.ProductName = (await _productService.GetProductByIdAsync(faqGroup.ProductId))?.Name ?? "N/A";

                return faqGroupModel;
            });
        });

        return model;
    }

    public virtual async Task<FaqGroupModel> PrepareFaqGroupModelAsync(FaqGroupModel model, FaqGroup faqGroup, bool excludeProperties = false)
    {
        if (faqGroup != null)
        {
            if (model == null)
            {
                model = faqGroup.ToModel<FaqGroupModel>();
                model.Name = await _localizationService.GetLocalizedAsync(faqGroup, f => f.Name);
            }

            model.ProductName = (await _productService.GetProductByIdAsync(faqGroup.ProductId))?.Name ?? string.Empty;
        }

        if (faqGroup == null && !excludeProperties)
        {
            model.Published = true;
        }

        var products = await _productService.SearchProductsAsync(pageSize: 50, showHidden: true);
        model.AvailableProducts = products.Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Text = p.Name,
            Value = p.Id.ToString(),
            Selected = faqGroup != null && p.Id == faqGroup.ProductId
        }).ToList();
        model.AvailableProducts.Insert(0, new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "---", Value = "0" });

        return model;
    }

    #endregion

    #region FaqItem

    public virtual async Task<FaqItemSearchModel> PrepareFaqItemSearchModelAsync(FaqItemSearchModel searchModel, FaqGroup faqGroup)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.FaqGroupId = faqGroup?.Id ?? 0;
        searchModel.SetGridPageSize();

        return searchModel;
    }

    public virtual async Task<FaqItemListModel> PrepareFaqItemListModelAsync(FaqItemSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var items = await _faqService.GetAllFaqItemsAsync(
            faqGroupId: searchModel.FaqGroupId,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        var model = await new FaqItemListModel().PrepareToGridAsync(searchModel, items, () =>
        {
            return items.SelectAwait(async faqItem =>
            {
                var faqItemModel = faqItem.ToModel<FaqItemModel>();

                faqItemModel.Question = await _localizationService.GetLocalizedAsync(faqItem, f => f.Question);
                faqItemModel.Answer = await _localizationService.GetLocalizedAsync(faqItem, f => f.Answer);

                var group = await _faqService.GetFaqGroupByIdAsync(faqItem.FaqGroupId);
                faqItemModel.GroupName = group != null ? await _localizationService.GetLocalizedAsync(group, g => g.Name) : string.Empty;

                return faqItemModel;
            });
        });

        return model;
    }

    public virtual async Task<FaqItemModel> PrepareFaqItemModelAsync(FaqItemModel model, FaqItem faqItem, bool excludeProperties = false)
    {
        if (faqItem != null)
        {
            if (model == null)
            {
                model = faqItem.ToModel<FaqItemModel>();
                model.Question = await _localizationService.GetLocalizedAsync(faqItem, f => f.Question);
                model.Answer = await _localizationService.GetLocalizedAsync(faqItem, f => f.Answer);
            }

            model.GroupName = (await _faqService.GetFaqGroupByIdAsync(faqItem.FaqGroupId))?.Name ?? string.Empty;
        }

        if (faqItem == null && !excludeProperties)
        {
            model.Published = true;
        }

        return model;
    }

    #endregion

    #endregion
}