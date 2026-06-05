using Nop.Core.Caching;
using Nop.Core.Domain.Configuration;
using Nop.Core.Events;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.ManufacturerShowcase.Infrastructure.Cache;

public class ModelCacheEventConsumer :
    IConsumer<EntityInsertedEvent<Setting>>,
    IConsumer<EntityUpdatedEvent<Setting>>,
    IConsumer<EntityDeletedEvent<Setting>>
{
    #region Fields

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager)
    {
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Methods

    public async Task HandleEventAsync(EntityInsertedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(ManufacturerListPrefix);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(ManufacturerListPrefix);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(ManufacturerListPrefix);
    }

    #endregion

    #region Properties

    protected static string ManufacturerListPrefix => "Nop.plugins.widgets.manufacturershowcase";

    public static CacheKey PictureUrlModelKey => new("Nop.plugins.widgets.manufacturershowcase.pictureurl-{0}-{1}");

    #endregion
}