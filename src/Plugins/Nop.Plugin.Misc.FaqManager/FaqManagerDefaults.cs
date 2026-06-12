using Nop.Core.Caching;

namespace Nop.Plugin.Misc.FaqManager;

public class FaqManagerDefaults
{
    public static string SystemName => "Misc.FaqManager";

    public static string FaqGroupsMenuSystemName => "FaqGroups";

    #region Caching defaults

    public static CacheKey FaqGroupByProductCacheKey => new("Nop.faq.group.by.product-{0}");
    public static CacheKey FaqItemsByGroupCacheKey => new("Nop.faq.items.by.group-{0}");
    public static string FaqPrefixCacheKey => "Nop.faq";

    #endregion

    #region Routes

    public static class Routes
    {
        private const string ROUTE_PREFIX = "Plugin.Misc.FaqManager.Route.";

        public static class Admin
        {
            public static string ConfigurationRouteName => ROUTE_PREFIX + "Configure";
            public static string FaqGroupsRouteName => ROUTE_PREFIX + "FaqGroups";
            public static string FaqGroupCreateRouteName => ROUTE_PREFIX + "FaqGroupCreate";
            public static string FaqGroupEditRouteName => ROUTE_PREFIX + "FaqGroupEdit";
            public static string FaqItemsRouteName => ROUTE_PREFIX + "FaqItems";
            public static string FaqItemEditRouteName => ROUTE_PREFIX + "FaqItemEdit";
            public static string FaqItemCreateRouteName => ROUTE_PREFIX + "FaqItemCreate";
        }

        public static class Public
        {
            public static string FaqGroupArchive => ROUTE_PREFIX + "FaqGroupArchive";
        }

        public static string FaqGroupIdRouteValue => "faqGroupId";
        public static string FaqItemIdRouteValue => "faqItemId";
    }

    #endregion

    #region Permissions

    public static class Permissions
    {
        public const string FAQ_VIEW = "FaqManager.View";
        public const string FAQ_MANAGE = "FaqManager.Manage";
    }

    #endregion

    #region Activity log types

    public static class ActivityLogTypeSystemNames
    {
        public static string AddNewFaqGroup => "AddNewFaqGroup";
        public static string EditFaqGroup => "EditFaqGroup";
        public static string DeleteFaqGroup => "DeleteFaqGroup";
        public static string AddNewFaqItem => "AddNewFaqItem";
        public static string EditFaqItem => "EditFaqItem";
        public static string DeleteFaqItem => "DeleteFaqItem";
    }

    #endregion
}