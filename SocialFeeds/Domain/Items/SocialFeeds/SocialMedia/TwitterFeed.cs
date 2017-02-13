#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia
{
    public partial class TwitterFeed : CustomItem
    {

        public static readonly string TemplateId = "{3400511E-8C16-4504-BAF2-13F387EF6603}";

        private readonly BaseFeed _BaseFeed;
        public BaseFeed BaseFeed { get { return _BaseFeed; } }

        #region Boilerplate CustomItem Code

        public TwitterFeed(Item innerItem) : base(innerItem)
        {
            _BaseFeed = new BaseFeed(innerItem);
        }

        public static implicit operator TwitterFeed(Item innerItem)
        {
            return innerItem != null ? new TwitterFeed(innerItem) : null;
        }

        public static implicit operator Item(TwitterFeed customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods

        public MultilistField Hashtags
        {
            get
            {
                return new MultilistField(InnerItem.Fields["Twitter Hashtags"]);
            }
        }

        public LookupField TwitterAccount
        {
            get
            {
                return new LookupField(InnerItem.Fields["Twitter Account"]);
            }
        }

        #endregion //Field Instance Methods
    }
}