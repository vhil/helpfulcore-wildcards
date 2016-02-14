using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Helpfulcore.Wildcards
{
    public class WildcardRouteItem
    {
        public static class FieldNames
        {
            public const string WildcardItems = "Wildcard Items";
            public const string UrlGenerationRules = "Url Generation Rules";
            public const string ItemResolvingRules = "Item Resolving Rules";
            public const string ItemTemplates = "Item Templates";
			public const string ItemSearchRootNode = "Item Search Root Node";
        }

        protected readonly Item Item;

        public WildcardRouteItem(Item wildcardRouteItem)
        {
            this.Item = wildcardRouteItem;
        }

        private IDictionary<int, string> urlGenerationRules;
        public IDictionary<int, string> UrlGenerationRules
        {
            get
            {
                return this.urlGenerationRules ??
                       (this.urlGenerationRules = this.GetNameValueList(FieldNames.UrlGenerationRules));
            }
        }

        private IDictionary<int, string> itemResolvingRules;
        public IDictionary<int, string> ItemResolvingRules
        {
            get
            {
                return this.itemResolvingRules ??
                       (this.itemResolvingRules = this.GetNameValueList(FieldNames.ItemResolvingRules));
            }
        }

        private IEnumerable<TemplateID> itemTemplates;
        public IEnumerable<TemplateID> ItemTemplates
        {
            get
            {
                return this.itemTemplates ?? (this.itemTemplates = this.GetMultilist(FieldNames.ItemTemplates)
                    .Select(x => new TemplateID(x))
                    .ToArray());
            }
        }

        private IEnumerable<Item> wildcardItems;
        public IEnumerable<Item> WildcardItems
        {
            get
            {
                return this.wildcardItems ?? (this.wildcardItems = this.WildcardItemIds
                    .Select(x => this.Item.Database.GetItem(x))
                    .ToArray());
            }
        }

        private IEnumerable<ID> wildcardItemIds;
        public IEnumerable<ID> WildcardItemIds
        {
            get { return this.wildcardItemIds ?? (this.wildcardItemIds = GetMultilist(FieldNames.WildcardItems)); }
        }

        private Item itemSearchRootNode;
        public Item ItemSearchRootNode
        {
            get { return this.itemSearchRootNode ?? (this.itemSearchRootNode = this.GetInternalLink(FieldNames.ItemSearchRootNode)); }
        }

        protected virtual Item GetInternalLink(string fieldName)
        {
            var field = (InternalLinkField)this.Item.Fields[fieldName];
            if (field == null)
            {
                throw new Exception(string.Format("Route item is invalid. There is no '{0}' field.", fieldName));
            }

            if (field.TargetID == ID.Null)
            {
                throw new Exception(string.Format("'{0}' field should not be empty on wildcard route item.", fieldName));
            }

            return field.TargetItem;
        }

        protected virtual IEnumerable<ID> GetMultilist(string fieldName)
        {
            var field = (MultilistField)this.Item.Fields[fieldName];
            if (field == null)
            {
                throw new Exception(string.Format("Route item is invalid. There is no '{0}' field.", fieldName));
            }

            if (!field.TargetIDs.Any())
            {
                throw new Exception(string.Format("'{0}' field should not be empty on wildcard route item.", fieldName));
            }

            return field.TargetIDs;
        }

        protected virtual IDictionary<int, string> GetNameValueList(string fieldName)
        {
            var field = (NameValueListField)this.Item.Fields[fieldName];
            if (field == null)
            {
                throw new Exception(string.Format("Route item is invalid. There is no '{0}' name value list field.", fieldName));
            }

            var ret = new Dictionary<int, string>();


            foreach (var ruleKey in field.NameValues.AllKeys)
            {
                int wildcardIndex;
                if (!int.TryParse(ruleKey, out wildcardIndex))
                {
                    throw new Exception(string.Format("'{0}' field contains invalid integer value in key.", fieldName));
                }

                ret.Add(wildcardIndex, field.NameValues[ruleKey]);
            }

            return ret;
        }
    }
}
