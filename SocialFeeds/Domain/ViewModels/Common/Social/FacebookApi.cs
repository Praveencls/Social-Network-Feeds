namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class FacebookApi : BaseFeedApi
    {
        public string ApiId { get; set; }
        public string Icon { get; set; }

        public FacebookApi()
        {
            ApiId = "";
            Icon = "";
        }
    }
}