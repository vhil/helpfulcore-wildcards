using System;
using System.Configuration.Provider;
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
			return (item.Paths.FullPath + "/").Contains("/*/");
		}
	}
}
