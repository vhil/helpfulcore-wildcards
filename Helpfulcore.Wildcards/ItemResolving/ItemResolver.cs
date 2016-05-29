using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;

namespace Helpfulcore.Wildcards.ItemResolving
{
    public class ItemResolver : ProviderBase
    {
        protected string WildcardTokenizedString
        {
            get { return Settings.GetSetting("WildcardTokenizedString", ",-w-,"); }
        }

        public virtual Item ResolveItem(Item item, WildcardRouteItem routeItem)
        {
            return null;
        }

        protected virtual IDictionary<string, string> GetTokenValues(Item item, WildcardRouteItem routeItem)
        {
            var ret = new Dictionary<string, string>();
            string[] itemUrl;

            using (new SiteContextSwitcher(Context.Site))
            {
	            var options = UrlOptions.DefaultOptions;
				options.LanguageEmbedding = LanguageEmbedding.Never;
	            var url = LinkManager.GetItemUrl(item, options);
	            itemUrl = url.Split('/');
            }

            var requestUrl = HttpContext.Current.Request.Url.LocalPath.Split('/');
            var rules = routeItem.ItemResolvingRules;

            int ruleIndex = 0;
            int index = 0;

            foreach (var rurl in requestUrl)
            {
                if (itemUrl[index] == this.WildcardTokenizedString)
                {
	                if (rules.ContainsKey(ruleIndex))
	                {
						var mapping = rules[ruleIndex];
						if (mapping == null)
						{
							throw new Exception("Can't resolve wildcards by index " + (ruleIndex));
						}

		                mapping = HttpUtility.UrlDecode(mapping);
		                if (!string.IsNullOrEmpty(mapping))
		                {
			                if (!ret.ContainsKey(mapping))
			                {
				                ret.Add(mapping, rurl);
			                }
		                }
	                }

					ruleIndex++;
				}

				index++;
            }

            return ret;
        }
    }
}
