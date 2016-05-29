using System.Web;
using Helpfulcore.Wildcards.ItemResolving;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Response.GetPageItem;

namespace Helpfulcore.Wildcards.Pipelines.Response.GetPageItem
{
	public class GetFromWildcard : GetPageItemProcessor
	{
		public override void Process(GetPageItemArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			if (args.Result != null)
			{
				if (!WildcardManager.Current.HasWildcardsPath(args.Result))
				{
					return;
				}
			}

			var originalItemCacheKey = "Wildcards.OriginalItem";

			HttpContext.Current.Items[originalItemCacheKey] = args.Result;
			
			args.Result = this.ResolveItem(args);
		}

		protected virtual Item ResolveItem(GetPageItemArgs args)
		{
            if (args.Result == null)
            {
                return null;
            }

		    var route = WildcardManager.Current.GetWildcardRouteForItemResolver(args.Result, Context.Site);
		    return WildcardItemResolver.Current.ResolveItem(args.Result, route);
		}
    }
}
