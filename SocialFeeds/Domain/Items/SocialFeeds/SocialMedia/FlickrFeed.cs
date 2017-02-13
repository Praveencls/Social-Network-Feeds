#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia
{
    public partial class FlickrFeed : CustomItem
    {

        public static readonly string TemplateId = "{F47902E7-CB18-42C3-9A7C-EAC0F332EF4D}";

        private readonly BaseFeed _BaseFeed;
        public BaseFeed BaseFeed { get { return _BaseFeed; } }


        #region Boilerplate CustomItem Code

        public FlickrFeed(Item innerItem) : base(innerItem)
        {
            _BaseFeed = new BaseFeed(innerItem);
        }

        public static implicit operator FlickrFeed(Item innerItem)
        {
            return innerItem != null ? new FlickrFeed(innerItem) : null;
        }

        public static implicit operator Item(FlickrFeed customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods

        public LookupField FlickrAccount
        {
            get
            {
                return new LookupField(InnerItem.Fields["Flickr Account"]);
            }
        }

        #endregion //Field Instance Methods
    }
}