#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia.Accounts
{
    public partial class TwitterAccount : CustomItem
    {

        public static readonly string TemplateId = "{0859317A-8759-41FA-88FC-C01B36C1BDA6}";


        #region Boilerplate CustomItem Code

        public TwitterAccount(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator TwitterAccount(Item innerItem)
        {
            return innerItem != null ? new TwitterAccount(innerItem) : null;
        }

        public static implicit operator Item(TwitterAccount customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods


        public TextField TwitterToken
        {
            get
            {
                return new TextField(InnerItem.Fields["Twitter Token"]);
            }
        }


        public TextField TwitterTokenSecret
        {
            get
            {
                return new TextField(InnerItem.Fields["Twitter Token Secret"]);
            }
        }

        public TextField TwitterConsumerKey
        {
            get
            {
                return new TextField(InnerItem.Fields["Twitter Consumer Key"]);
            }
        }

        public TextField TwitterConsumerSecret
        {
            get
            {
                return new TextField(InnerItem.Fields["Twitter Consumer Secret"]);
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