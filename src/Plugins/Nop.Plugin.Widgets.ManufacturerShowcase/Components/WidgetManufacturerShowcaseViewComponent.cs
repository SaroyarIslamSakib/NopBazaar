using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.ManufacturerShowcase.Infrastructure.Cache;
using Nop.Plugin.Widgets.ManufacturerShowcase.Models;
using Nop.Services.Catalog;
using Nop.Services.Helpers;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.ManufacturerShowcase.Components;

public class WidgetManufacturerShowcaseViewComponent : NopViewComponent
{
    #region Fields

    protected readonly IManufacturerService _manufacturerService;
    protected readonly IPictureService _pictureService;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly IStoreContext _storeContext;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWebHelper _webHelper;

    #endregion

    #region Ctor

    public WidgetManufacturerShowcaseViewComponent(
        IManufacturerService manufacturerService,
        IPictureService pictureService,
        IStaticCacheManager staticCacheManager,
        IStoreContext storeContext,
        IUrlRecordService urlRecordService,
        IWebHelper webHelper)
    {
        _manufacturerService = manufacturerService;
        _pictureService = pictureService;
        _staticCacheManager = staticCacheManager;
        _storeContext = storeContext;
        _urlRecordService = urlRecordService;
        _webHelper = webHelper;
    }

    #endregion

    #region Utilities

    private async Task<string> GetPictureUrlAsync(int pictureId)
    {
        if (pictureId == 0)
            return string.Empty;

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
            ModelCacheEventConsumer.PictureUrlModelKey,
            pictureId, _webHelper.IsCurrentConnectionSecured());

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var url = await _pictureService.GetPictureUrlAsync(pictureId, targetSize: 120, showDefaultPicture: false) ?? "";
            return url;
        });
    }

    #endregion

    #region Methods

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var manufacturers = await _manufacturerService.GetAllManufacturersAsync(
            storeId: store.Id,
            pageSize: int.MaxValue);

        if (!manufacturers.Any())
            return Content("");

        var model = new PublicInfoModel
        {
            ShowImages = true,
            ShowNames = true,
        };

        foreach (var manufacturer in manufacturers)
        {
            var picUrl = await GetPictureUrlAsync(manufacturer.PictureId);
            var seName = await _urlRecordService.GetSeNameAsync(manufacturer);

            model.Manufacturers.Add(new()
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                SeName = seName,
                PictureUrl = picUrl,
                LazyLoading = true
            });
        }

        if (!model.Manufacturers.Any())
            return Content("");

        return await ViewAsync("~/Plugins/Widgets.ManufacturerShowcase/Views/PublicInfo.cshtml", model);
    }

    #endregion
}