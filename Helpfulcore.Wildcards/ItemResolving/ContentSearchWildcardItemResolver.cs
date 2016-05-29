using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;

namespace Helpfulcore.Wildcards.ItemResolving
{
    public class ContentSearchWildcardItemResolver : ItemResolver
    {
	    private IDictionary<string, string> encodeNameReplacements;
	    protected virtual IDictionary<string, string> EncodeNameReplacements
	    {
		    get { return this.encodeNameReplacements ?? (this.encodeNameReplacements = this.GetEncodeNameReplacements()); }
	    }

	    protected bool WildcardWrapSearchTermsInEncodedQuotes
	    {
		    get { return Settings.GetBoolSetting("WildcardWrapSearchTermsInEncodedQuotes", false); }
	    }

		protected bool WildcardTokenizeSearchTerms
		{
			get { return Settings.GetBoolSetting("WildcardTokenizeSearchTerms", false); }
		}

		public override Item ResolveItem(Item item, WildcardRouteItem routeItem)
        {
            if (routeItem == null)
            {
                return item;
            }

            var tokenValues = this.GetTokenValues(item, routeItem);
			
			var rootItem = routeItem.ItemSearchRootNode;
            
			using (var searchContext = ContentSearchManager.CreateSearchContext((SitecoreIndexableItem) item))
            {
				var queryable = searchContext.GetQueryable<SearchResultItem>();
				
                var predicate = PredicateBuilder.True<SearchResultItem>();

				foreach (var itemTemplateId in routeItem.ItemTemplates)
                {
                    predicate = predicate.Or(p => p.TemplateId == itemTemplateId.ID);
                }

	            queryable = queryable
					.Where(predicate)
					.Where(resultItem => resultItem.Language == Context.Language.Name);

				if (rootItem != null)
				{
					queryable = queryable.Where(resultItem => resultItem.Paths.Contains(rootItem.ID));
				}

				foreach (var tokenRule in tokenValues)
				{
					queryable = this.AddTokenPredicates(tokenRule, queryable);
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

		protected IQueryable<SearchResultItem> AddTokenPredicates(KeyValuePair<string, string> tokenRule, IQueryable<SearchResultItem> queryable)
        {
			var predicate = PredicateBuilder.True<SearchResultItem>();
			var indexFieldName = this.GetPropertyIndexName(tokenRule.Key);
	        var searchTerm = this.GetTokenSearchibleValue(tokenRule.Value);

			foreach (var replacement in this.EncodeNameReplacements)
	        {
		        var value = replacement.Value;
		        if (searchTerm.ToLower().Contains(value.ToLower()))
		        {
					foreach (var name in this.EncodeNameReplacements.Where(x => x.Value == value))
					{
						var searchTermVariant = searchTerm.Replace(name.Value, name.Key);
						predicate = predicate.Or(p => p[indexFieldName] == searchTermVariant);
					}
		        }
	        }

			predicate = predicate.Or(p => p[indexFieldName] == searchTerm);

	        return queryable.Where(predicate);
        }

        protected virtual string GetTokenSearchibleValue(string value)
        {
	        if (this.WildcardTokenizeSearchTerms)
	        {
		        value = value
			        .Replace("-", "")
			        .Replace("+", "")
			        .Replace(" ", "")
					.ToLower();

	        }

	        if (this.WildcardWrapSearchTermsInEncodedQuotes)
	        {
				return "%22" + value + "%22";
	        }

	        return value;
        }

        protected virtual string GetPropertyIndexName(string key)
        {
            return key.ToLower().Replace(" ", "_").Replace("-", "_");
        }

	    protected virtual IDictionary<string, string> GetEncodeNameReplacements()
	    {
			var ret = new Dictionary<string, string>();

			var nameReplacements = Factory.GetConfigNode("encodeNameReplacements", false);

			if (nameReplacements != null)
		    {
				foreach (XmlNode replacementNode in nameReplacements.ChildNodes)
			    {
				    if (replacementNode.Attributes != null)
				    {
					    var modeAttr = replacementNode.Attributes["mode"];
						var findAttr = replacementNode.Attributes["find"];
						var replaceWithAttr = replacementNode.Attributes["replaceWith"];
						if (modeAttr != null && findAttr != null && replaceWithAttr != null)
					    {
						    var mode = modeAttr.Value;
							var find = findAttr.Value;
							var replaceWith = replaceWithAttr.Value;

							if ("on".Equals(mode, StringComparison.InvariantCultureIgnoreCase) && !ret.ContainsKey(find) && !string.IsNullOrEmpty(find))
						    {
								ret.Add(find, replaceWith);
						    }
					    }
				    }
				    
			    }
		    }

		    return ret;
	    }
    }
}
