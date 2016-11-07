using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html.Admin
{
    public static class NotificationExtensions
    {
        public static MvcHtmlString DisplayNotifications(this HtmlHelper helper, IDictionary<string, object> data)
        {
            var builder = new TagBuilder("ul");
            builder.AddCssClass("messages");

            builder.InnerHtml += GetNotificationsByType(data, NotifyType.Error);
            builder.InnerHtml += GetNotificationsByType(data, NotifyType.Success);
            builder.InnerHtml += GetNotificationsByType(data, NotifyType.Warning);

            return builder.ToMvcHtmlString();
        }

        internal static string GetNotificationsByType(IDictionary<string, object> data, NotifyType type)
        {
            List<string> notifications = (List<string>)data[string.Format("falcon.notifications.{0}", type)];
            if (notifications != null && notifications.Count > 0)
            {
                var liTag = new TagBuilder("li");
                liTag.AddCssClass(type.ToString().ToLower() + "-msg");

                var ulTag = new TagBuilder("ul");

                var notifyTag = new TagBuilder("li");
                foreach (string notification in notifications)
                {
                    notifyTag.InnerHtml = notification;
                    ulTag.InnerHtml += notifyTag.ToString();
                }
                liTag.InnerHtml = ulTag.ToString();

                return liTag.ToString();
            }
            else
            {
                return "";
            }

        }
    }
}
