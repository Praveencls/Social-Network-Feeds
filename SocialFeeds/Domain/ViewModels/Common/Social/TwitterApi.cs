namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class TwitterApi : BaseFeedApi
    {
        public string HashTagsWithTokens { get; set; }
        public string Icon { get; set; }

        public TwitterApi()
        {
            HashTagsWithTokens = "";
            Icon = "";
        }
    }
}