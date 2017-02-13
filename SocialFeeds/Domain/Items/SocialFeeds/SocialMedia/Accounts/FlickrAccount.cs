#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia.Accounts
{
    public partial class FlickrAccount : CustomItem
    {

        public static readonly string TemplateId = "{3D3DD041-AF34-46EC-A802-CDA9BF6F3298}";


        #region Boilerplate CustomItem Code

        public FlickrAccount(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator FlickrAccount(Item innerItem)
        {
            return innerItem != null ? new FlickrAccount(innerItem) : null;
        }

        public static implicit operator Item(FlickrAccount customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods


        public TextField AccountId
        {
            get
            {
                return new TextField(InnerItem.Fields["Account ID"]);
            }
        }


        public LookupField SocialLink
        {
            get
            {
                return new LookupField(InnerItem.Fields["Social Link"]);
            }
        }
        
        #endregion //Field Instance Methods
    }
}