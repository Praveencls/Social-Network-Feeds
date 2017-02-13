#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia
{
    public partial class InstagramFeed : CustomItem
    {

        public static readonly string TemplateId = "{559CF6D5-4141-4A9A-BBFE-A261A19F0578}";

        private readonly BaseFeed _BaseFeed;
        public BaseFeed BaseFeed { get { return _BaseFeed; } }

        #region Boilerplate CustomItem Code

        public InstagramFeed(Item innerItem) : base(innerItem)
        {
            _BaseFeed = new BaseFeed(innerItem);
        }

        public static implicit operator InstagramFeed(Item innerItem)
        {
            return innerItem != null ? new InstagramFeed(innerItem) : null;
        }

        public static implicit operator Item(InstagramFeed customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods

        public MultilistField Hashtags
        {
            get
            {
                return new MultilistField(InnerItem.Fields["Hashtags"]);
            }
        }

        public LookupField InstagramAccount
        {
            get
            {
                return new LookupField(InnerItem.Fields["Instagram Account"]);
            }
        }

        #endregion //Field Instance Methods
    }
}