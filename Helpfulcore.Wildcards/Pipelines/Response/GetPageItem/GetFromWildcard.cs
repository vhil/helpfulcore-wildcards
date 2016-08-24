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
        private const string OriginalItemCacheKey = "Wildcards.OriginalItem";

        public override void Process(GetPageItemArgs args)
		{
            if (!this.WildvardResolvingPossible(args))
            {
                return;
            }

            HttpContext.Current.Items[OriginalItemCacheKey] = args.Result;

            var resolvedItem = this.ResolveItem(args);

            if (resolvedItem != null)
            {
                args.Result = resolvedItem;
            }
		}

	    protected virtual bool WildvardResolvingPossible(GetPageItemArgs args)
	    {
	        if (args == null || args.Result == null || Context.Site == null || Context.Database == null || Context.Database.Name == "core")
	        {
	            return false;
	        }

	        if (!WildcardManager.Current.HasWildcardsPath(args.Result))
	        {
	            return false;
	        }

	        return true;
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
