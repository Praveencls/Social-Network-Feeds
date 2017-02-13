#region

using Sitecore.Data.Fields;
using Sitecore.Data.Items;

#endregion

namespace SocialFeeds.Domain.Items.SocialFeeds.Links
{
    public partial class Hashtag : CustomItem
    {

        public static readonly string TemplateId = "{CE17F811-DFEB-44ED-8F1F-9CEE448095A0}";


        #region Boilerplate CustomItem Code

        public Hashtag(Item innerItem) : base(innerItem)
        {

        }

        public static implicit operator Hashtag(Item innerItem)
        {
            return innerItem != null ? new Hashtag(innerItem) : null;
        }

        public static implicit operator Item(Hashtag customItem)
        {
            return customItem != null ? customItem.InnerItem : null;
        }

        #endregion //Boilerplate CustomItem Code


        #region Field Instance Methods

        public TextField Value
        {
            get
            {
                return new TextField(InnerItem.Fields["Value"]);
            }
        }

        #endregion //Field Instance Methods
    }
}