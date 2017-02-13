#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using SocialFeeds.Domain.Items.SocialFeeds.SocialMedia;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.Links
{
    public partial class SocialMedia : CustomItem
    {

        public static readonly string TemplateId = "{8CE78279-D09B-4AA8-8DE3-A2A54A3FBE5F}";


        #region Boilerplate CustomItem Code

        public SocialMedia(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator SocialMedia(Item innerItem)
        {
            return innerItem != null ? new SocialMedia(innerItem) : null;
        }

        public static implicit operator Item(SocialMedia customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods

        public ImageField SocialIcon
        {
            get
            {
                return new ImageField(InnerItem.Fields["Icon"]);
            }
        }
        
        #endregion //Field Instance Methods
    }
}