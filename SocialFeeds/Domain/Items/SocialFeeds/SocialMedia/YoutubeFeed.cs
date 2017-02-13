#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia
{
    public partial class YoutubeFeed : CustomItem
    {

        public static readonly string TemplateId = "{856B42A0-8BBE-47C5-BF69-1DE5AD29259E}";

        private readonly BaseFeed _BaseFeed;
        public BaseFeed BaseFeed { get { return _BaseFeed; } }

        #region Boilerplate CustomItem Code

        public YoutubeFeed(Item innerItem) : base(innerItem)
        {
            _BaseFeed = new BaseFeed(innerItem);
        }

        public static implicit operator YoutubeFeed(Item innerItem)
        {
            return innerItem != null ? new YoutubeFeed(innerItem) : null;
        }

        public static implicit operator Item(YoutubeFeed customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods
        
        public LookupField YouTubeAccount
        {
            get
            {
                return new LookupField(InnerItem.Fields["YouTube Account"]);
            }
        }

        #endregion //Field Instance Methods
    }
}