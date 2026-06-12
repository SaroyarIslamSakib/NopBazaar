using Nop.Plugin.Misc.FaqManager.Domain;
using Nop.Services.Caching;

namespace Nop.Plugin.Misc.FaqManager.Services.Caching;

public class FaqGroupCacheEventConsumer : CacheEventConsumer<FaqGroup>
{
    protected override async Task ClearCacheAsync(FaqGroup entity, EntityEventType entityEventType)
    {
        await RemoveByPrefixAsync(FaqManagerDefaults.FaqPrefixCacheKey);
    }
}