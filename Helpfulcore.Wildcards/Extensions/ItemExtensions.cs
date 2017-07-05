using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Helpfulcore.Wildcards.Extensions
{
	internal static class ItemExtensions
	{
		internal static IEnumerable<Item> GetChildrenReccursively(this Item item, Guid? templateId = null, params Guid[] reccursionStoperTemplates)
		{
			var result = new List<Item>();

			foreach (Item child in item.GetChildren(ChildListOptions.IgnoreSecurity))
			{
				if (!templateId.HasValue || child.IsDerived(new ID(templateId.Value)))
				{
					result.Add(child);
				}

				if (!reccursionStoperTemplates.Contains(child.TemplateID.Guid))
				{
					result.AddRange(child.GetChildrenReccursively(templateId, reccursionStoperTemplates));
				}
			}

			return result;
		}

		internal static bool IsDerived(this Item item, ID templateId)
		{
			if (item == null) return false;

			return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
		}

		private static bool IsDerived(this Item item, Item templateItem)
		{
			if (item == null || templateItem == null) return false;

			var itemTemplate = TemplateManager.GetTemplate(item);
			return itemTemplate != null && (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
		}
	}
}
