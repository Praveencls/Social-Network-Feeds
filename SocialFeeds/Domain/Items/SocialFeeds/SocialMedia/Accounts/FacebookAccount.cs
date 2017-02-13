#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia.Accounts
{
    public partial class FacebookAccount : CustomItem
    {

        public static readonly string TemplateId = "{C3749412-BFA3-463A-808D-FCF0FEC5D146}";


        #region Boilerplate CustomItem Code

        public FacebookAccount(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator FacebookAccount(Item innerItem)
        {
            return innerItem != null ? new FacebookAccount(innerItem) : null;
        }

        public static implicit operator Item(FacebookAccount customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods


        public TextField ApiId
        {
            get
            {
                return new TextField(InnerItem.Fields["API ID"]);
            }
        }


        public TextField ApiKey
        {
            get
            {
                return new TextField(InnerItem.Fields["API Key"]);
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