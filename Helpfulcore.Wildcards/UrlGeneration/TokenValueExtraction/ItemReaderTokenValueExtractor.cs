using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Helpfulcore.Wildcards.UrlGeneration.TokenValueExtraction
{
    public class ItemReaderTokenValueExtractor : TokenValueExtractor
    {
        public override string ExtractTokenValue(string tokenPattern, Item item)
        {
            if (string.IsNullOrEmpty(tokenPattern))
            {
                return string.Empty;
            }

            var tokens = HttpUtility.UrlDecode(tokenPattern)
                .Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            return ExtractValue(tokens, item);
           
        }

        protected virtual string ExtractValue(ICollection<string> tokens, object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            if (!tokens.Any())
            {
                if (obj is Item)
                {
                    return ((Item) obj).Name;
                }

                return obj.ToString();
            }

            var tokenPattern = tokens.FirstOrDefault();

            if (string.IsNullOrEmpty(tokenPattern))
            {
                return string.Empty;
            }

            object value = string.Empty;

            // this is a property
            if (tokenPattern.StartsWith("@@"))
            {
                tokenPattern = tokenPattern.Replace("@@", string.Empty);
                value = this.GetProperty(tokenPattern, obj);
            }
            // this is a field
            else if (tokenPattern.StartsWith("@"))
            {
                tokenPattern = tokenPattern.Replace("@", string.Empty);
                value = this.GetItemField(tokenPattern, obj);
            }

            var remainingTokens = tokens.Skip(1).ToArray();

            if (value is Item)
            {
                return ExtractValue(remainingTokens, value as Item);
            }

            if (!(value is string))
            {
                return this.ExtractValue(remainingTokens, value);
            }

            return value.ToString();
        }

        protected virtual object GetItemField(string fieldName, object item)
        {
            if (!(item is Item))
            {
                throw new WildcardException("Given object is not an item.");
            }

            var castedItem = (Item) item;

            object fieldValue = castedItem[fieldName];
            var fieldValiueItem = castedItem.Database.GetItem(fieldValue.ToString());

            if (fieldValiueItem != null)
            {
                return fieldValiueItem;
            }

            return fieldValue;
        }

        protected virtual object GetProperty(string propertyName, object obj)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public override string HelpText
        {
            get
            {
                return "Prefix your fields with '@'. For example: @__Display Name <br/>" +
                       "Prefix your properties with '@@. For example: @@Name <br/>" +
                       "Use '.' delimiter for creating a chain <br/>" +
                       "<h3>Examples</h3>" +
                       "<ul>" +
                       "<li>@@Name</li>" +
                       "<li>@__Display Name</li>" +
                       "<li>@@Paths.@@Path</li>" +
                       "<li>@@Parent.@@Name</li>" +
                       "<li>@Custom Link Field.@@Name</li>" +
                       "<li>@@Template.@@InnerItem.@__Standard values.@@Paths.@@Path</li>" +
                       "</ul>" +
                       "<h3>Notes:</h3>" +
                       "Property names ARE case sensitive <br/>" +
                       "Field names are NOT case sensitive";
            }
        }
    }
}