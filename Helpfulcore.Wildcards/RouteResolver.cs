using System;
using System.Configuration.Provider;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Sites;

namespace Helpfulcore.Wildcards
{
	public class RouteResolver : ProviderBase
	{
		public virtual WildcardRouteItem GetWildcardRouteForItemResolver(Item item, SiteContext site)
		{
			throw new NotImplementedException();
		}

		public virtual WildcardRouteItem GetWildcardRouteForLinkProvider(Item item, SiteContext site)
		{
			throw new NotImplementedException();
		}

		public virtual void ClearCache()
		{
			throw new NotImplementedException();
		}

		public virtual bool HasWildcardsPath(Item item)
		{
			var path = item.Paths.Path
				.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
				.ToDictionary(k => k, v => v);

			return path.ContainsKey("*");
		}
	}
}
