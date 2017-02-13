#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia
{
    public partial class FacebookFeed : CustomItem
    {

        public static readonly string TemplateId = "{ECA84AD5-F2F5-4472-BE23-089B21E72F23}";

        #region Inherited Base Templates

        private readonly BaseFeed _BaseFeed;
        public BaseFeed BaseFeed { get { return _BaseFeed; } }

        #endregion

        #region Boilerplate CustomItem Code

        public FacebookFeed(Item innerItem) : base(innerItem)
        {
            _BaseFeed = new BaseFeed(innerItem);
        }

        public static implicit operator FacebookFeed(Item innerItem)
        {
            return innerItem != null ? new FacebookFeed(innerItem) : null;
        }

        public static implicit operator Item(FacebookFeed customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods
        
        public LookupField FacebookAccount
        {
            get
            {
                return new LookupField(InnerItem.Fields["Facebook Account"]);
            }
        }

        #endregion //Field Instance Methods
    }
}