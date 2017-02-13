#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia
{
    public partial class PinterestFeed : CustomItem
    {

        public static readonly string TemplateId = "{176794E8-3BB9-4ACB-9F2C-95C0CF0669AC}";

        private readonly BaseFeed _BaseFeed;
        public BaseFeed BaseFeed { get { return _BaseFeed; } }

        #region Boilerplate CustomItem Code

        public PinterestFeed(Item innerItem) : base(innerItem)
        {
            _BaseFeed = new BaseFeed(innerItem);
        }

        public static implicit operator PinterestFeed(Item innerItem)
        {
            return innerItem != null ? new PinterestFeed(innerItem) : null;
        }

        public static implicit operator Item(PinterestFeed customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods
        
        public LookupField PinterestAccount
        {
            get
            {
                return new LookupField(InnerItem.Fields["Pinterest Account"]);
            }
        }

        #endregion //Field Instance Methods
    }
}