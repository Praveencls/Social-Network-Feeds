namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class PinterestApi : BaseFeedApi
    {
        public string AccountId { get; set; }
        public string Icon { get; set; }

        public PinterestApi()
        {
            AccountId = "";
            Icon = "";
        }
    }
}