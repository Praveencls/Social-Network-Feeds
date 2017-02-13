#region

using Facebook;
using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using SocialFeeds.Domain.Extensions;
using SocialFeeds.Domain.ViewModels.Common;
using SocialFeeds.Domain.ViewModels.Common.Social;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

#endregion

namespace socialfeeds.Controllers.SocialFeeds
{
    public class SocialFeedController : Controller, IController
    {
        private string FaceBookFeedParams = Sitecore.Configuration.Settings.GetSetting("FacebookFeedsParams", "id,name,picture,message,story,link,place,full_picture,updated_time,from{name,id,picture,link}");
        private string FaceBookAPIVersion = Sitecore.Configuration.Settings.GetSetting("FacebookAPIVersion", "v2.8");

        /// <summary>
        /// Controller for facebook and twitter based on type parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limit"></param>
        /// <param name="feed"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Get(string id, string limit, string feed, string type)
        {
            if (type == "facebook")
            {
                return GetFacebookJson(id, limit, feed);
            }
            if (type == "twitter")
            {
                return GetTwitterJson(id, limit);
            }

            return this.Json(null);
        }

        /// <summary>
        /// Gets facebook feeds from Facebook API version 1.1
        /// </summary>
        /// <param name="id">facebook user id</param>
        /// <param name="limit">facebook feed count</param>
        /// <param name="feed"></param>
        /// <returns></returns>
        public ActionResult GetFacebookJson(string id, string limit, string feed)
        {
            var userId = id.Split('|')[0];
            var accessCode = id.Split('|')[1];
            var api = new FacebookAPI(accessCode);
            // Facebook attributes used as a query string includes in facebook API
            var attributes = new Dictionary<string, string>
            {
                {"limit", limit},
                {"fields", FaceBookFeedParams}
            };
            // Get all facebook feeds from API
            var facebookFeeds = api.Get("/"+ FaceBookAPIVersion + "/" + userId + "/" + feed, attributes);
            List<FacebookFeedItem> socialFeeds = new List<FacebookFeedItem>();
            if (facebookFeeds.Dictionary["data"] != null && facebookFeeds.Dictionary["data"].Array.Length > 0)
            {
                JSONObject[] jsonArray = facebookFeeds.Dictionary["data"].Array;
                foreach (JSONObject jsonObj in jsonArray)
                {
                    if (jsonObj != null)
                    {
                        // Create new instance of facebook feed class
                        var socialFeed = new FacebookFeedItem();
                        // Fill facebook feed name
                        if (jsonObj.Dictionary.ContainsKey("from") && jsonObj.Dictionary["from"].Dictionary.ContainsKey("name"))
                        {
                            socialFeed.Title = jsonObj.Dictionary["from"].Dictionary["name"].String;
                        }

                        // Fill facebook feed picture
                        if (jsonObj.Dictionary.ContainsKey("from") && jsonObj.Dictionary["from"].Dictionary.ContainsKey("picture"))
                        {
                            socialFeed.Thumb = new Image()
                            {
                                Src = jsonObj.Dictionary["from"].Dictionary["picture"].Dictionary["data"].Dictionary["url"].String,
                                Alt = "Thumbnail"
                            };
                        }

                        // Fill facebook feed full picture
                        if (jsonObj.Dictionary.ContainsKey("full_picture") && !jsonObj.Dictionary["full_picture"].String.IsNullOrEmpty())
                        {
                            socialFeed.FullPicture = new Image()
                            {
                                Src = jsonObj.Dictionary["full_picture"].String,
                                Alt = "Image"
                            };
                        }

                        // Fill facebook feed link
                        if (jsonObj.Dictionary.ContainsKey("from") && jsonObj.Dictionary["from"].Dictionary.ContainsKey("link"))
                        {
                            socialFeed.Link = new Link()
                            {
                                Url = jsonObj.Dictionary["from"].Dictionary["link"].String
                            };
                        }

                        // Fill facebook feed message
                        if (jsonObj.Dictionary.ContainsKey("message") && !jsonObj.Dictionary["message"].String.IsNullOrEmpty())
                        {
                            socialFeed.Content = Regex.Replace(jsonObj.Dictionary["message"].String, @"\r\n?|\n", "<br />");
                        }
                        // Fill facebook feed last updated time
                        if (jsonObj.Dictionary.ContainsKey("updated_time") && !jsonObj.Dictionary["updated_time"].String.IsNullOrEmpty())
                        {
                            var dateTime = DateTime.Parse(jsonObj.Dictionary["updated_time"].String).ToString("MMMM dd,yyyy, hh:mm tt");
                            socialFeed.DateLocation = dateTime;
                            socialFeed.Date = dateTime;
                        }

                        // Fill facebook feed place
                        if (jsonObj.Dictionary.ContainsKey("place") && jsonObj.Dictionary["place"].Dictionary.ContainsKey("location"))
                        {
                            var place = jsonObj.Dictionary["place"].Dictionary["location"].Dictionary["city"].String + ", " + jsonObj.Dictionary["place"].Dictionary["location"].Dictionary["country"].String;
                            socialFeed.DateLocation = socialFeed.DateLocation + " near " + place;
                        }
                        // Add new instance of facebook feed in facebook feeds
                        socialFeeds.Add(socialFeed);
                    }
                }
            }
            // Serialize social feeds instance into json string
            return this.Json(socialFeeds, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get tweets json from twitter API
        /// </summary>
        /// <param name="id">Twitter id</param>
        /// <param name="limit">Twitter tweets count</param>
        /// <returns></returns>
        public ActionResult GetTwitterJson(string id, string limit)
        {
            // oauth application keys
            string[] twitterKeysArray = id.Split('|');
            
            try
            {
                if (twitterKeysArray.Length == 5)
                {
                    var screenname = twitterKeysArray[0];
                    var oauthToken = twitterKeysArray[1];
                    var oauthTokenSecret = twitterKeysArray[2];
                    var oauthConsumerKey = twitterKeysArray[3];
                    var oauthConsumerSecret = twitterKeysArray[4];
                    int count = Convert.ToInt32(limit);
                    // oauth implementation details
                    var oauth_version = "1.0";
                    var oauth_signature_method = "HMAC-SHA1";

                    // unique request details
                    var oauthNonce = Convert.ToBase64String(
                        new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

                    var timeSpan = DateTime.UtcNow
                                   - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                    var oauthTimestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

                    // message api details
                    var status = "Updating status via REST API if this works";

                    // Base url for getting twitter tweets
                    var resourceUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json";

                    // create oauth signature
                    var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                     "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

                    var baseString = string.Format(baseFormat,
                        oauthConsumerKey,
                        oauthNonce,
                        oauth_signature_method,
                        oauthTimestamp,
                        oauthToken,
                        oauth_version,
                        Uri.EscapeDataString(screenname)
                        );

                    baseString = string.Concat("GET&", Uri.EscapeDataString(resourceUrl), "&",
                        Uri.EscapeDataString(baseString));

                    var compositeKey = string.Concat(Uri.EscapeDataString(oauthConsumerSecret),
                        "&", Uri.EscapeDataString(oauthTokenSecret));

                    string oauth_signature;

                    using (HMACSHA1 hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey)))
                    {
                        oauth_signature = Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(baseString)));
                    }

                    // create the request header
                    var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                                       "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                                       "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                                       "oauth_version=\"{6}\"";

                    var authHeader = string.Format(headerFormat,
                        Uri.EscapeDataString(oauthNonce),
                        Uri.EscapeDataString(oauth_signature_method),
                        Uri.EscapeDataString(oauthTimestamp),
                        Uri.EscapeDataString(oauthConsumerKey),
                        Uri.EscapeDataString(oauthToken),
                        Uri.EscapeDataString(oauth_signature),
                        Uri.EscapeDataString(oauth_version)
                        );

                    // make the request
                    ServicePointManager.Expect100Continue = false;

                    var postBody = "screen_name=" + Uri.EscapeDataString(screenname);
                    resourceUrl += "?" + postBody;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resourceUrl);
                    request.Headers.Add("Authorization", authHeader);
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";

                    // Binding list of twitter feed class from tweets from twitter api
                    WebResponse response = request.GetResponse();
                    string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var jsonArray = JArray.Parse(responseData);
                    var jsonChildren = jsonArray.Children<JObject>().Take(count);
                    List<TwitterTweetItem> twitterTweets = new List<TwitterTweetItem>();
                    foreach (JObject o in jsonChildren)
                    {
                        if (o != null)
                        {
                            // create new instance of twitter tweet class
                            var tweetItem = new TwitterTweetItem();

                            // Fill content property from text property from twitter json
                            if (o.Property("text") != null && !o.Property("text").Value.ToString().IsNullOrEmpty())
                            {
                                tweetItem.Content = o.Property("text").Value.ToString();
                            }

                            // Fill date property from created at property from twitter json
                            if (o.Property("created_at") != null &&
                                !o.Property("created_at").Value.ToString().IsNullOrEmpty())
                            {
                                tweetItem.Date = o.Property("created_at").Value.ToString();
                            }

                            // Fill profile_image, screen_name and account_name property from user json object from twitter json
                            if (o.Property("user") != null && o.Property("user").Children<JObject>().Any())
                            {
                                // Get all user properties
                                var usrObjectProperties = o.Property("user").Children<JObject>().Properties();
                                if (usrObjectProperties.Any())
                                {
                                    // Fill profile_image property from user json object
                                    var profileImageProperty =
                                        usrObjectProperties.FirstOrDefault(i => i.Name.Equals("profile_image_url"));
                                    if (profileImageProperty != null &&
                                        !profileImageProperty.Value.ToString().IsNullOrEmpty())
                                    {
                                        tweetItem.Thumb = new Image()
                                        {
                                            Src = profileImageProperty.Value.ToString(),
                                            Alt = "Profile Thumbnail"
                                        };
                                    }

                                    // Fill screen_name property from user json object
                                    var screenProperty =
                                        usrObjectProperties.FirstOrDefault(i => i.Name.Equals("screen_name"));
                                    if (screenProperty != null && !screenProperty.Value.ToString().IsNullOrEmpty())
                                    {
                                        var searchUrl = "http://twitter.com/intent/user?screen_name={0}";
                                        var screenName = screenProperty.Value.ToString();
                                        tweetItem.Link = new Link()
                                        {
                                            Url = string.Format(searchUrl, screenName),
                                            Text = string.Format("@{0}", screenName)
                                        };
                                    }

                                    // Fill name property from user json object
                                    var nameProperty = usrObjectProperties.FirstOrDefault(i => i.Name.Equals("name"));
                                    if (nameProperty != null && !nameProperty.Value.ToString().IsNullOrEmpty())
                                    {
                                        tweetItem.Title = nameProperty.Value.ToString();
                                    }
                                }
                            }
                            // Add new instance of twitter tweet class in twitter tweets
                            twitterTweets.Add(tweetItem);
                        }
                    }
                    // Serialize twitter tweets instance into json string
                    return this.Json(twitterTweets, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error Getting Tweets from Twitter :" + ex.Message, this);
            }

            return this.Json(null);
        }
    }
}