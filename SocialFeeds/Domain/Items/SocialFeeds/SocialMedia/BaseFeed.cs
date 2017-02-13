#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia
{
    public partial class BaseFeed : CustomItem
    {

        public static readonly string TemplateId = "{6A024D16-E04B-4B1D-8944-FA6929F75203}";


        #region Boilerplate CustomItem Code

        public BaseFeed(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator BaseFeed(Item innerItem)
        {
            return innerItem != null ? new BaseFeed(innerItem) : null;
        }

        public static implicit operator Item(BaseFeed customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods

        public TextField FeedPriority
        {
            get
            {
                return new TextField(InnerItem.Fields["Feed Priority"]);
            }
        }

        #endregion //Field Instance Methods
    }
}