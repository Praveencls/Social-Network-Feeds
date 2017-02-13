namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class YouTubeApi : BaseFeedApi
    {
        public string AccountId { get; set; }
        public string AccountApiKey { get; set; }
        public string Icon { get; set; }

        public YouTubeApi()
        {
            AccountId = "";
            AccountApiKey = "";
            Icon = "";
        }
    }
}