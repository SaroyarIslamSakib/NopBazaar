using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Services.Caching;

namespace Nop.Plugin.Misc.FaqManager.Services.Caching;

public class FaqItemCacheEventConsumer : CacheEventConsumer<FaqItem>
{
    protected override async Task ClearCacheAsync(FaqItem entity, EntityEventType entityEventType)
    {
        await RemoveByPrefixAsync(FaqManagerDefaults.FaqPrefixCacheKey);
    }
}