namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class FlickrApi : BaseFeedApi
    {
        public string AccountId { get; set; }
        public string Icon { get; set; }

        public FlickrApi()
        {
            AccountId = "";
            Icon = "";
        }
    }
}