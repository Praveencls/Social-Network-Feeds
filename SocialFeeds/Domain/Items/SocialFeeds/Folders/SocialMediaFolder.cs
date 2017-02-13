#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using SocialFeeds.Domain.Items.SocialFeeds.SocialMedia;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.Folders
{
    public partial class SocialMediaFolder : CustomItem
    {

        public static readonly string TemplateId = "{806344CC-54C1-42DA-B9DA-26A5A6AE8DE0}";


        #region Boilerplate CustomItem Code

        public SocialMediaFolder(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator SocialMediaFolder(Item innerItem)
        {
            return innerItem != null ? new SocialMediaFolder(innerItem) : null;
        }

        public static implicit operator Item(SocialMediaFolder customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods

        public TextField NumberofPosts
        {
            get
            {
                return new TextField(InnerItem.Fields["Number of Posts"]);
            }
        }

        #endregion //Field Instance Methods
    }
}