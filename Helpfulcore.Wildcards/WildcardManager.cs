using System.Collections.Generic;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Sites;

namespace Helpfulcore.Wildcards
{
    public class WildcardManager
    {
        protected static IDictionary<string, WildcardRoute> ItemResolverRoutesCache = new Dictionary<string, WildcardRoute>();
        protected static IDictionary<string, WildcardRoute> LinkProviderRoutesCache = new Dictionary<string, WildcardRoute>();

        protected static string QueryForItemResolver
        {
            get { return RoutesPath + "/*[contains(@#" + WildcardRoute.FieldNames.WildcardItems + "#, '{0}')]"; }
        }

        protected static string QueryForLinkProvider
        {
            get { return RoutesPath + "/*[contains(@#" + WildcardRoute.FieldNames.ItemTemplates + "#, '{0}')]"; }
        }

        protected static string RoutesPath
        {
            get { return Settings.GetSetting("WildcardProvider.RoutesPath", "/sitecore/system/modules/wildcards/routes"); }
        }

        public static WildcardRoute GetWildcardRouteForItemResolver(Item item, SiteContext site)
        {
            Assert.ArgumentNotNull(item, "item");

            var cacheKey = string.Format("cache{0}_{1}_{2}", item.ID, item.Language.Name, site.Name);
            if (ItemResolverRoutesCache.ContainsKey(cacheKey))
            {
                return ItemResolverRoutesCache[cacheKey];
            }

            var query = string.Format(QueryForItemResolver, item.ID);
            var route = Context.Database.SelectSingleItem(query);

            if (route == null)
            {
                return null;
            }

            var wildcardRoute = new WildcardRoute(route);

            ItemResolverRoutesCache.Add(cacheKey, wildcardRoute);

            return wildcardRoute;
        }

        public static WildcardRoute GetWildcardRouteForLinkProvider(Item item, SiteContext site)
        {
            Assert.ArgumentNotNull(item, "item");

            var cacheKey = string.Format("cache{0}_{1}_{2}", item.TemplateID, item.Language.Name, site.Name);
            if (LinkProviderRoutesCache.ContainsKey(cacheKey))
            {
                return LinkProviderRoutesCache[cacheKey];
            }

            var query = string.Format(QueryForLinkProvider, item.TemplateID);
            var route = Context.Database.SelectSingleItem(query);

            if (route == null)
            {
                return null;
            }

            var wildcardRoute = new WildcardRoute(route);

            LinkProviderRoutesCache.Add(cacheKey, wildcardRoute);

            return wildcardRoute;
        }

        public static void ClearCache()
        {
            ItemResolverRoutesCache.Clear();
            LinkProviderRoutesCache.Clear();
        }
    }
}
