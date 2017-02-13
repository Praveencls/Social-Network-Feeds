namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class TwitterTweetItem
    {
        public Image Thumb { get; set; }

        public Link Link { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }
        
        public string Content { get; set; }
    }
}