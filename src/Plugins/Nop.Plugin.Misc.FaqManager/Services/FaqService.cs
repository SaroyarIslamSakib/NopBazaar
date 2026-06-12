using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Services;

public class FaqService
{
    #region Fields

    private readonly IEventPublisher _eventPublisher;
    private readonly IRepository<FaqGroup> _faqGroupRepository;
    private readonly IRepository<FaqItem> _faqItemRepository;
    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public FaqService(IEventPublisher eventPublisher,
        IRepository<FaqGroup> faqGroupRepository,
        IRepository<FaqItem> faqItemRepository,
        IStaticCacheManager staticCacheManager)
    {
        _eventPublisher = eventPublisher;
        _faqGroupRepository = faqGroupRepository;
        _faqItemRepository = faqItemRepository;
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region FaqGroup

    public async Task DeleteFaqGroupAsync(FaqGroup faqGroup)
    {
        ArgumentNullException.ThrowIfNull(faqGroup);

        var faqItems = await _faqItemRepository.GetAllAsync(query =>
            query.Where(fi => fi.FaqGroupId == faqGroup.Id));

        foreach (var faqItem in faqItems)
            await _faqItemRepository.DeleteAsync(faqItem);

        await _faqGroupRepository.DeleteAsync(faqGroup);

        await _eventPublisher.EntityDeletedAsync(faqGroup);
    }

    public async Task<FaqGroup> GetFaqGroupByIdAsync(int faqGroupId)
    {
        return await _faqGroupRepository.GetByIdAsync(faqGroupId, cache => default);
    }

    public async Task<FaqGroup?> GetFaqGroupByProductIdAsync(int productId)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
            FaqManagerDefaults.FaqGroupByProductCacheKey, productId);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var groups = await _faqGroupRepository.GetAllAsync(query =>
                query.Where(fg => fg.ProductId == productId && fg.Published));

            return groups.FirstOrDefault();
        });
    }

    public async Task<IPagedList<FaqGroup>> GetAllFaqGroupsAsync(int productId = 0,
        bool showHidden = false, int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var groups = await _faqGroupRepository.GetAllPagedAsync(query =>
        {
            if (productId > 0)
                query = query.Where(fg => fg.ProductId == productId);

            if (!showHidden)
                query = query.Where(fg => fg.Published);

            query = query.OrderBy(fg => fg.DisplayOrder).ThenBy(fg => fg.Id);

            return query;
        }, pageIndex, pageSize);

        return groups;
    }

    public async Task InsertFaqGroupAsync(FaqGroup faqGroup)
    {
        ArgumentNullException.ThrowIfNull(faqGroup);

        await _faqGroupRepository.InsertAsync(faqGroup);

        await _eventPublisher.EntityInsertedAsync(faqGroup);
    }

    public async Task UpdateFaqGroupAsync(FaqGroup faqGroup)
    {
        ArgumentNullException.ThrowIfNull(faqGroup);

        await _faqGroupRepository.UpdateAsync(faqGroup);

        await _eventPublisher.EntityUpdatedAsync(faqGroup);
    }

    #endregion

    #region FaqItem

    public async Task DeleteFaqItemAsync(FaqItem faqItem)
    {
        ArgumentNullException.ThrowIfNull(faqItem);

        await _faqItemRepository.DeleteAsync(faqItem);

        await _eventPublisher.EntityDeletedAsync(faqItem);
    }

    public async Task<FaqItem> GetFaqItemByIdAsync(int faqItemId)
    {
        return await _faqItemRepository.GetByIdAsync(faqItemId, cache => default);
    }

    public async Task<IList<FaqItem>> GetFaqItemsByGroupIdAsync(int faqGroupId, bool showHidden = false)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(
            FaqManagerDefaults.FaqItemsByGroupCacheKey, faqGroupId, showHidden);

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var items = await _faqItemRepository.GetAllAsync(query =>
            {
                query = query.Where(fi => fi.FaqGroupId == faqGroupId);

                if (!showHidden)
                    query = query.Where(fi => fi.Published);

                query = query.OrderBy(fi => fi.DisplayOrder).ThenBy(fi => fi.Id);

                return query;
            });

            return items;
        });
    }

    public async Task<IPagedList<FaqItem>> GetAllFaqItemsAsync(int faqGroupId = 0,
        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var items = await _faqItemRepository.GetAllPagedAsync(query =>
        {
            if (faqGroupId > 0)
                query = query.Where(fi => fi.FaqGroupId == faqGroupId);

            query = query.OrderBy(fi => fi.DisplayOrder).ThenBy(fi => fi.Id);

            return query;
        }, pageIndex, pageSize);

        return items;
    }

    public async Task InsertFaqItemAsync(FaqItem faqItem)
    {
        ArgumentNullException.ThrowIfNull(faqItem);

        await _faqItemRepository.InsertAsync(faqItem);

        await _eventPublisher.EntityInsertedAsync(faqItem);
    }

    public async Task UpdateFaqItemAsync(FaqItem faqItem)
    {
        ArgumentNullException.ThrowIfNull(faqItem);

        await _faqItemRepository.UpdateAsync(faqItem);

        await _eventPublisher.EntityUpdatedAsync(faqItem);
    }

    #endregion
}