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

			if (args.Result != null && (args.Result).Name != "*")
			{
				return;
			}

			args.Result = this.ResolveItem(args);
		}

		protected virtual Item ResolveItem(GetPageItemArgs args)
		{
            if (args.Result == null)
            {
                return null;
            }

		    var route = WildcardManager.GetWildcardRouteForItemResolver(args.Result, Context.Site);
		    return WildcardItemResolver.Current.ResolveItem(args.Result, route);
		}
    }
}
