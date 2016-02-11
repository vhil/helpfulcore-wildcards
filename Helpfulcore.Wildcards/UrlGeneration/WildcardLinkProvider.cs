using System;
using System.Collections.Generic;
using System.Linq;
using Helpfulcore.Wildcards.UrlGeneration.TokenValueExtraction;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Helpfulcore.Wildcards.UrlGeneration
{
    public class WildcardLinkProvider : LinkProvider
    {
        protected string WildcardTokenizedString
        {
            get { return Settings.GetSetting("WildcardTokenizedString", ",-w-,"); }
        }

        public override string GetItemUrl(Item item, UrlOptions options)
        {
            var route = WildcardManager.GetWildcardRouteForLinkProvider(item, Context.Site);

            if (route == null || route.WildcardItemIds.Any(x => x == item.ID))
            {
                return base.GetItemUrl(item, options);
            }

            var url = base.GetItemUrl(item, options);
            var routeUrl = this.GetMostSuitableRootItemUrl(route, options);

            if (string.IsNullOrEmpty(routeUrl))
            {
                return url;
            }

            return this.ReplaceUrlTokens(routeUrl, item, route, options);
        }

        protected virtual string ReplaceUrlTokens(string routeUrl, Item item, WildcardRoute route, UrlOptions options)
        {
            var resultUrl = new List<string>();
            
            var tokenValues = this.GetTokenValues(item, route, options);
            
            var urlPattern = routeUrl.Split(new[] { '/' });
            var tokenCounter = 0;
            foreach (var segment in urlPattern)
            {
                var resultSegment = segment;
                if (segment.ToLower() == WildcardTokenizedString)
                {
                    resultSegment = tokenValues[tokenCounter++];
                }

                resultUrl.Add(resultSegment);
            }

            return string.Join("/", resultUrl);
        }

        protected virtual IDictionary<int, string> GetTokenValues(Item item, WildcardRoute route, UrlOptions options)
        {
            var ret = new Dictionary<int, string>();
            var rules = route.UrlGenerationRules;

            foreach (var rule in rules)
            {
                var mapping = rule.Value.Trim();

                var tokenValue = UrlGenerationTokenValueExtractor.Current.ExtractTokenValue(mapping, item);

                if (options.LowercaseUrls)
                {
                    tokenValue = tokenValue.ToLower();
                }

                tokenValue = tokenValue.Replace(" ", "-");

                ret.Add(rule.Key, tokenValue);
            }

            return ret;
        }

        protected virtual string GetMostSuitableRootItemUrl(WildcardRoute routeItem, UrlOptions options)
        {
            var url = string.Empty;

            var rootItems = routeItem.WildcardItems;
            var currentSiteItem = rootItems.FirstOrDefault(x => x.Paths.FullPath.StartsWith(Context.Site.RootPath, StringComparison.InvariantCultureIgnoreCase));

            if (currentSiteItem != null)
            {
                return base.GetItemUrl(currentSiteItem, options);
            }

            return url;
        }
    }
}
