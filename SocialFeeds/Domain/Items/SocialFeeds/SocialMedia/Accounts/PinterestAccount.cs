#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.SocialMedia.Accounts
{
    public partial class PinterestAccount : CustomItem
    {

        public static readonly string TemplateId = "{39BBE355-44A6-4D10-995B-27BA5E2E7D2E}";


        #region Boilerplate CustomItem Code

        public PinterestAccount(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator PinterestAccount(Item innerItem)
        {
            return innerItem != null ? new PinterestAccount(innerItem) : null;
        }

        public static implicit operator Item(PinterestAccount customItem)
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