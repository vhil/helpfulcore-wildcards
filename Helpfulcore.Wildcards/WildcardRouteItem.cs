using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Helpfulcore.Wildcards
{
    public class WildcardRouteItem
    {
		public static ID TemplateId = new ID("{B4C339CC-57FD-4FF2-ACBA-120B72C5FE78}");
        public static class FieldNames
        {
            public const string WildcardItems = "Wildcard Items";
            public const string UrlGenerationRules = "Url Generation Rules";
            public const string ItemResolvingRules = "Item Resolving Rules";
            public const string ItemTemplates = "Item Templates";
			public const string ItemSearchRootNode = "Item Search Root Node";
        }

        internal readonly Item Item;

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

        private ICollection<TemplateID> itemTemplates;
        public ICollection<TemplateID> ItemTemplates
        {
            get
            {
                return this.itemTemplates ?? (this.itemTemplates = this.GetMultilist(FieldNames.ItemTemplates)
                    .Select(x => new TemplateID(x))
                    .ToArray());
            }
        }

        private ICollection<Item> wildcardItems;
        public ICollection<Item> WildcardItems
        {
            get
            {
                return this.wildcardItems ?? (this.wildcardItems = this.WildcardItemIds
                    .Select(x => this.Item.Database.GetItem(x))
                    .ToArray());
            }
        }

        private ICollection<ID> wildcardItemIds;
        public ICollection<ID> WildcardItemIds
        {
            get { return this.wildcardItemIds ?? (this.wildcardItemIds = this.GetMultilist(FieldNames.WildcardItems).ToArray()); }
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
                throw new WildcardException(string.Format("Route item is invalid. There is no '{0}' field.", fieldName));
            }

            if (field.TargetID == ID.Null)
            {
                throw new WildcardException(string.Format("'{0}' field should not be empty on wildcard route item.", fieldName));
            }

            return field.TargetItem;
        }

        protected virtual IEnumerable<ID> GetMultilist(string fieldName)
        {
            var field = (MultilistField)this.Item.Fields[fieldName];
            if (field == null)
            {
                throw new WildcardException(string.Format("Route item is invalid. There is no '{0}' field.", fieldName));
            }

            if (!field.TargetIDs.Any())
            {
                throw new WildcardException(string.Format("'{0}' field should not be empty on wildcard route item.", fieldName));
            }

            return field.TargetIDs;
        }

        protected virtual IDictionary<int, string> GetNameValueList(string fieldName)
        {
            var field = (NameValueListField)this.Item.Fields[fieldName];
            if (field == null)
            {
                throw new WildcardException(string.Format("Route item is invalid. There is no '{0}' name value list field.", fieldName));
            }

            var ret = new Dictionary<int, string>();


            foreach (var ruleKey in field.NameValues.AllKeys)
            {
                int wildcardIndex;
                if (!int.TryParse(ruleKey, out wildcardIndex))
                {
                    throw new WildcardException(string.Format("'{0}' field contains invalid integer value in key.", fieldName));
                }

                ret.Add(wildcardIndex, field.NameValues[ruleKey]);
            }

            return ret;
        }
    }
}
