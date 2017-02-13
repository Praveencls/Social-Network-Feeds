using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;

namespace SocialFeeds.Domain.Extensions
{
    public static class ItemExtensions
    {
        /// <summary>
        /// Resolve the item URL
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetUrl(this Item item)
        {
            return Sitecore.Links.LinkManager.GetItemUrl(item);
        }

        /// <summary>
        /// Determines if this item is of the provided TemplateId
        /// </summary>
        /// <param name="item"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static bool IsOfType(this Item item, string templateId)
        {
            return item.IsOfType(new ID(templateId));
        }
        
        /// <summary>
        /// Determines if this item is of the provided TemplateId
        /// </summary>
        /// <param name="item"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static bool IsOfType(this Item item, ID templateId) 
        {
            return item.TemplateID.Equals(templateId);
        }

        public static bool InheritsFromType(this Item item, string templateId)
        {
            return InheritsFromType(item, ID.Parse(templateId));
        }

        public static bool InheritsFromType(this Item item, ID templateId)
        {
            try {
                return item.IsOfType(templateId)
                    || GetTemplate(item).InheritsFrom(templateId);
            }
            catch
            {
                return false;
            }
        }

        public static bool InheritsTemplate(this Item item, string templateId) {

            return InheritsFromType(item, templateId);
        }

        /// <summary>
        /// Gets the Template of an item (which is different than the TemplateItem)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Template GetTemplate(this Item item)
        {
            return TemplateManager.GetTemplate(item);
        }


        public static Template GetTemplate(this string item) {
            return TemplateManager.GetTemplate(Sitecore.Context.Database.GetItem(item));
        }

        /// <summary>
        /// Filter Item collection by context language version
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<Item> FilterByContextLanguageVersion(this IEnumerable<Item> collection)
        {
            return collection.Where(i => i.Versions.Count > 0);
        }
        /// <summary>
        /// Checks if item has version in context language or not
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool HasContextLanguageVersion(this Item item)
        {
            return item != null && item.Versions.Count > 0;
        }

        /// <summary>
        /// This returns a list of child items based on a list of templates names provided
        /// </summary>
        /// <param name="parent">
        /// Parent Item to search for children
        /// </param>
        /// <param name="templateNames">
        /// The list of template names to look for
        /// </param>
        /// <returns>
        /// Returns a list of items that match the templatenames provided
        /// </returns>
        public static List<Item> ChildrenByTemplates(this Item parent, List<string> templateNames) {

            try {
                return (from child in parent.GetChildren().ToArray() where templateNames.Contains(child.TemplateName) select child).ToList();
            }
            catch (Exception ex) {
                return null;
            }
        }

    }

}
