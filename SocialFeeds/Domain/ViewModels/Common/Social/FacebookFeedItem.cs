namespace SocialFeeds.Domain.ViewModels.Common.Social
{
    public class FacebookFeedItem
    {
        public Image Thumb { get; set; }

        public Image FullPicture { get; set; }

        public Link Link { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string DateLocation { get; set; }

        public string Content { get; set; }
    }
}