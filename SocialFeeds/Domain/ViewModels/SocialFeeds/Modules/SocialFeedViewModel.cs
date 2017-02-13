using System.Collections.Generic;
using System.Linq;
using Sitecore.Mvc.Presentation;
using SocialFeeds.Domain.Extensions;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Resources.Media;
using SocialFeeds.Domain.Items.SocialFeeds.Folders;
using SocialFeeds.Domain.Items.SocialFeeds.Links;
using SocialFeeds.Domain.Items.SocialFeeds.SocialMedia;
using SocialFeeds.Domain.Items.SocialFeeds.SocialMedia.Accounts;
using SocialFeeds.Domain.ViewModels.Common.Social;

namespace SocialFeeds.Domain.ViewModels.SocialFeeds.Modules
{
    public class SocialFeedViewModel : IRenderingModel
    {
        public FacebookApi FacebookApi { get; set; }
        public TwitterApi TwitterApi { get; set; }
        public InstagramApi InstagramApi { get; set; }
        public YouTubeApi YouTubeApi { get; set; }
        public PinterestApi PinterestApi { get; set; }

        public FlickrApi FlickrApi { get; set; }
        public SocialMediaFolder CurrentItem { get; set; }
        public bool ShowSocialFeed { get; set; }

        public void Initialize(Rendering rendering)
        {
            //Get current item
            var dataSource = rendering.DataSource;
            if (dataSource.IsNullOrEmpty())
                dataSource = Sitecore.Configuration.Settings.GetSetting("FeedsDataSource", "{AF7BCA0A-4C7F-4ACA-9956-E4801143775A}");

            CurrentItem = Sitecore.Context.Database.GetItem(dataSource);

            if (CurrentItem != null)
            {
                // Get all social feeds items
                var socialFeeds = CurrentItem.InnerItem.GetChildren().ToList();
                FacebookApi = new FacebookApi();
                TwitterApi = new TwitterApi();
                InstagramApi = new InstagramApi();
                YouTubeApi = new YouTubeApi();
                PinterestApi = new PinterestApi();
                FlickrApi = new FlickrApi();

                if (socialFeeds.Any())
                {
                    // Get facebook feed item and bind facebook api class properties
                    var facebookFeedItem = socialFeeds.FirstOrDefault(i => i.IsOfType(FacebookFeed.TemplateId));
                    if (facebookFeedItem != null)
                    {
                        var facebookFeed = new FacebookFeed(facebookFeedItem);
                        if (facebookFeed != null)
                        {
                            if (facebookFeed.FacebookAccount.TargetItem != null)
                            {
                                var facebookAccount = new FacebookAccount(facebookFeed.FacebookAccount.TargetItem);

                                if (facebookAccount.SocialLink != null)
                                {
                                    if (!ShowSocialFeed) ShowSocialFeed = true;
                                    FacebookApi.ApiId = string.Join("|",
                                        new string[]
                                        {
                                            facebookAccount.ApiId.Value, facebookAccount.ApiKey.Value
                                        });
                                    FacebookApi.Icon = facebookAccount.SocialLink != null
                                        ? MediaManager.GetMediaUrl(
                                            new SocialMedia(facebookAccount.SocialLink.TargetItem).SocialIcon.MediaItem)
                                        : "";
                                    // new BaseFeed()

                                    FacebookApi.Priority = GetPriority(facebookFeed.BaseFeed);
                                }
                            }
                        }
                    }
                    
                    // Get twitter feed item and bind twitter api class properties
                    var twitterFeedItem = socialFeeds.FirstOrDefault(i => i.IsOfType(TwitterFeed.TemplateId));
                    if (twitterFeedItem != null)
                    {
                        var twitterFeed = new TwitterFeed(twitterFeedItem);
                        if (twitterFeed != null)
                        {
                            if (twitterFeed.TwitterAccount.TargetItem != null)
                            {
                                var twitterAccount = new TwitterAccount(twitterFeed.TwitterAccount.TargetItem);

                                if (!ShowSocialFeed) ShowSocialFeed = true;
                                TwitterApi.HashTagsWithTokens = string.Join("|", new string[]
                                {
                                    string.Join(",",
                                        twitterFeed.Hashtags.GetItems().Select(i => new Hashtag(i).Value.Value)),
                                    twitterAccount.TwitterToken.Value,
                                    twitterAccount.TwitterTokenSecret.Value,
                                    twitterAccount.TwitterConsumerKey.Value,
                                    twitterAccount.TwitterConsumerSecret.Value
                                });

                                TwitterApi.Icon = twitterAccount.SocialLink != null
                                    ? MediaManager.GetMediaUrl(
                                        new SocialMedia(twitterAccount.SocialLink.TargetItem).SocialIcon.MediaItem)
                                    : "";

                                TwitterApi.Priority = GetPriority(twitterFeed.BaseFeed);
                            }
                        }
                    }

                    // Get instagram feed item and bind instagram api class properties
                    var instagramFeedItem = socialFeeds.FirstOrDefault(i => i.IsOfType(InstagramFeed.TemplateId));

                    if (instagramFeedItem != null)
                    {
                        var instagramFeed = new InstagramFeed(instagramFeedItem);
                        if (instagramFeed != null)
                        {
                            if (instagramFeed.InstagramAccount.TargetItem != null)
                            {
                                var instagramAccount = new InstagramAccount(instagramFeed.InstagramAccount.TargetItem);
                                
                                if (!ShowSocialFeed) ShowSocialFeed = true;
                                InstagramApi.HashTags = string.Join(",",
                                    instagramFeed.Hashtags.GetItems().Select(i => new Hashtag(i).Value.Value));
                                InstagramApi.AccessToken = instagramAccount.AccessToken.Value;
                                InstagramApi.ClientId = instagramAccount.InstagramClientId.Value;
                                InstagramApi.Icon = instagramAccount.SocialLink != null
                                    ? MediaManager.GetMediaUrl(
                                        new SocialMedia(instagramAccount.SocialLink.TargetItem).SocialIcon.MediaItem)
                                    : "";


                                InstagramApi.Priority = GetPriority(instagramFeed.BaseFeed);

                            }
                        }
                    }


                    // Get youtube feed item and bind youtube api class properties
                    var youTubeFeedItem = socialFeeds.FirstOrDefault(i => i.IsOfType(YoutubeFeed.TemplateId));
                    if (youTubeFeedItem != null)
                    {
                        var youTubeFeed = new YoutubeFeed(youTubeFeedItem);
                        if (youTubeFeed.YouTubeAccount.TargetItem != null)
                        {
                            var youtubeAccount = new YoutubeAccount(youTubeFeed.YouTubeAccount.TargetItem);

                            if (!ShowSocialFeed) ShowSocialFeed = true;
                            YouTubeApi.AccountId = youtubeAccount.AccountId.Value;
                            YouTubeApi.AccountApiKey = youtubeAccount.AccountApiKey.Value;

                            YouTubeApi.Icon = youtubeAccount.SocialLink != null
                                ? MediaManager.GetMediaUrl(
                                    new SocialMedia(youtubeAccount.SocialLink.TargetItem).SocialIcon.MediaItem)
                                : "";
                            YouTubeApi.Priority = GetPriority(youTubeFeed.BaseFeed);

                        }
                    }

                    // Get pinterest feed item and bind pinterest api class properties
                    var pinterestFeedItem = socialFeeds.FirstOrDefault(i => i.IsOfType(PinterestFeed.TemplateId));
                    if (pinterestFeedItem != null)
                    {
                        var pinterestFeed = new PinterestFeed(pinterestFeedItem);
                        if (pinterestFeed != null && pinterestFeed.PinterestAccount.TargetItem != null)
                        {

                            var pinterestAccount = new PinterestAccount(pinterestFeed.PinterestAccount.TargetItem);


                            if (!ShowSocialFeed) ShowSocialFeed = true;
                            PinterestApi.AccountId = pinterestAccount.AccountId.Value;

                            PinterestApi.Icon = pinterestAccount.SocialLink != null
                                ? MediaManager.GetMediaUrl(
                                    new SocialMedia(pinterestAccount.SocialLink.TargetItem).SocialIcon.MediaItem)
                                : "";
                            PinterestApi.Priority = GetPriority(pinterestFeed.BaseFeed);

                        }
                    }

                    // Get flickr feed item and bind flickr api class properties
                    var flickrFeedItem = socialFeeds.FirstOrDefault(i => i.IsOfType(FlickrFeed.TemplateId));
                    if (flickrFeedItem != null)
                    {
                        var flickrFeed = new FlickrFeed(flickrFeedItem);
                        if (flickrFeed != null)
                        {
                            if (flickrFeed.FlickrAccount.TargetItem != null)
                            {
                                var flickrAccount = new FlickrAccount(flickrFeed.FlickrAccount.TargetItem);

                                if (!ShowSocialFeed) ShowSocialFeed = true;
                                FlickrApi.AccountId = flickrAccount.AccountId.Value;
                                FlickrApi.Icon = flickrAccount.SocialLink != null
                                    ? MediaManager.GetMediaUrl(
                                        new SocialMedia(flickrAccount.SocialLink.TargetItem).SocialIcon.MediaItem)
                                    : "";

                                FlickrApi.Priority = GetPriority(flickrFeed.BaseFeed);
                            }
                        }
                    }
                }
            }
        }

        public string GetPriority(BaseFeed baseFeed)
        {
            return !baseFeed.FeedPriority.Value.IsNullOrEmpty() && !baseFeed.FeedPriority.Value.Any(x => char.IsLetter(x)) ? baseFeed.FeedPriority.Value : System.Int16.MaxValue.ToString();
        }
    }
}