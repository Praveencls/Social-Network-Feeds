using System.Web;
using System.Web.Mvc;

namespace SocialFeeds.Domain.Extensions
{
    public static class DictonaryHelper
    {
        public static IHtmlString RenderDictionaryValue(string value)
        {
            return new MvcHtmlString(HttpUtility.HtmlDecode(Sitecore.Globalization.Translate.Text(value)));
        }
    }
}
