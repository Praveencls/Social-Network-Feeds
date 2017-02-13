namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class InstagramApi : BaseFeedApi
    {
        public string HashTags { get; set; }
        public string AccessToken { get; set; }
        public string ClientId { get; set; }
        public string Icon { get; set; }

        public InstagramApi()
        {
            HashTags = "";
            AccessToken = "";
            ClientId = "";
            Icon = "";
        }
    }
}