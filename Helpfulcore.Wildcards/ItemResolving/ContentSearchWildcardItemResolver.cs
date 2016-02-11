using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Security;
using Sitecore.Data.Items;

namespace Helpfulcore.Wildcards.ItemResolving
{
    public class ContentSearchWildcardItemResolver : ItemResolver
    {
        public override Item ResolveItem(Item item, WildcardRoute route)
        {
            if (route == null)
            {
                return item;
            }

            var rootItem = route.ItemRootNode;

            if (rootItem == null)
            {
                return item;
            }

            var tokenValues = this.GetTokenValues(item, route);

            var database = "web";
            if (Context.Database.Name == "master")
            {
                database = "master";
            }

            using (var searchContext = ContentSearchManager.GetIndex("sitecore_" + database + "_index").CreateSearchContext(SearchSecurityOptions.EnableSecurityCheck))
            {
                var predicate = PredicateBuilder.True<SearchResultItem>();
                foreach (var itemTemplateId in route.ItemTemplates)
                {
                    predicate = predicate.Or(p => p.TemplateId == itemTemplateId.ID);
                }

                var queryable = searchContext.GetQueryable<SearchResultItem>().Where(predicate)
                    .Where(resultItem => resultItem.Paths.Contains(route.ItemRootNode.ID) && resultItem.Language == Context.Language.Name);

                foreach (var tokenRule in tokenValues)
                {
                    queryable = this.ConvertRuleToQuery(tokenRule, queryable);
                }

                var result = queryable.FirstOrDefault();
                if (result == null)
                {
                    return item;
                }

                var resolvedItem = result.GetItem();
                if (resolvedItem != null)
                {
                    return resolvedItem;
                }
            }

            return item;
        }

        protected virtual IQueryable<SearchResultItem> ConvertRuleToQuery(KeyValuePair<string, string> tokenRule, IQueryable<SearchResultItem> queryable)
        {
            return queryable.Where(x =>
                x[this.GetPropertyIndexName(tokenRule.Key)] == this.GetTokenSearchibleValue(tokenRule.Value) 
            ||  x[this.GetPropertyIndexName(tokenRule.Key)] == this.GetTokenSearchibleValue(tokenRule.Value).Replace("-", " "));
        }

        protected virtual string GetTokenSearchibleValue(string value)
        {
            return "%22" + value + "*%22";
        }

        protected virtual string GetPropertyIndexName(string key)
        {
            return key.ToLower().Replace(" ", "_").Replace("-", "_");
        }
    }
}
