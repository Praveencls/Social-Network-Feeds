#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia.Accounts
{
    public partial class YoutubeAccount : CustomItem
    {

        public static readonly string TemplateId = "{7FFF4E33-E801-427B-B572-C966FFEDA366}";


        #region Boilerplate CustomItem Code

        public YoutubeAccount(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator YoutubeAccount(Item innerItem)
        {
            return innerItem != null ? new YoutubeAccount(innerItem) : null;
        }

        public static implicit operator Item(YoutubeAccount customItem)
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

        public TextField AccountApiKey
        {
            get
            {
                return new TextField(InnerItem.Fields["Account API Key"]);
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