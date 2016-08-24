using System.Collections.Generic;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Sites;

namespace Helpfulcore.Wildcards
{
	public class WildcardRouteResolver : RouteResolver
	{
		protected IDictionary<string, WildcardRouteItem> ItemResolverRoutesCache = new Dictionary<string, WildcardRouteItem>();
		protected IDictionary<string, WildcardRouteItem> LinkProviderRoutesCache = new Dictionary<string, WildcardRouteItem>();

		protected virtual string QueryForItemResolver
		{
			get { return this.RoutesPath + "/*[contains(@#" + WildcardRouteItem.FieldNames.WildcardItems + "#, '{0}')]"; }
		}

		protected virtual string QueryForLinkProvider
		{
			get { return this.RoutesPath + "/*[contains(@#" + WildcardRouteItem.FieldNames.ItemTemplates + "#, '{0}')]"; }
		}

		protected virtual string RoutesPath
		{
			get { return Settings.GetSetting("WildcardProvider.RoutesPath", "/sitecore/system/modules/wildcards/routes"); }
		}

		public override WildcardRouteItem GetWildcardRouteForItemResolver(Item item, SiteContext site)
		{
			Assert.ArgumentNotNull(item, "item");

			var cacheKey = string.Format("cache{0}_{1}_{2}", item.ID, item.Language.Name, site.Name);
			if (this.ItemResolverRoutesCache.ContainsKey(cacheKey))
			{
				return this.ItemResolverRoutesCache[cacheKey];
			}

			var query = string.Format(this.QueryForItemResolver, item.ID);
			var route = Context.Database.SelectSingleItem(query);

			if (route == null)
			{
				return null;
			}

			var wildcardRoute = new WildcardRouteItem(route);

			this.ItemResolverRoutesCache.Add(cacheKey, wildcardRoute);

			return wildcardRoute;
		}

		public override WildcardRouteItem GetWildcardRouteForLinkProvider(Item item, SiteContext site)
		{
			Assert.ArgumentNotNull(item, "item");

			var cacheKey = string.Format("cache{0}_{1}_{2}", item.TemplateID, item.Language.Name, site.Name);
			if (this.LinkProviderRoutesCache.ContainsKey(cacheKey))
			{
				return this.LinkProviderRoutesCache[cacheKey];
			}

			var query = string.Format(this.QueryForLinkProvider, item.TemplateID);
			var route = item.Database.SelectSingleItem(query);

			if (route == null)
			{
				return null;
			}

			var wildcardRoute = new WildcardRouteItem(route);

			this.LinkProviderRoutesCache.Add(cacheKey, wildcardRoute);

			return wildcardRoute;
		}

		public override void ClearCache()
		{
			this.ItemResolverRoutesCache.Clear();
			this.LinkProviderRoutesCache.Clear();
		}
	}
}
