#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia.Accounts
{
    public partial class InstagramAccount : CustomItem
    {

        public static readonly string TemplateId = "{A780BB3C-2E1D-4F43-B1F5-1FE02B58F548}";


        #region Boilerplate CustomItem Code

        public InstagramAccount(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator InstagramAccount(Item innerItem)
        {
            return innerItem != null ? new InstagramAccount(innerItem) : null;
        }

        public static implicit operator Item(InstagramAccount customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods


        public TextField AccessToken
        {
            get
            {
                return new TextField(InnerItem.Fields["Instagram Access Token"]);
            }
        }


        public TextField InstagramClientId
        {
            get
            {
                return new TextField(InnerItem.Fields["Instagram Client ID"]);
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