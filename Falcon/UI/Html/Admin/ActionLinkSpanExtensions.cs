using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Falcon.UI.Html.Admin
{
    public static class ActionLinkSpanExtensions
    {
        public static MvcHtmlString ActionLinkSpan(this HtmlHelper helper, string linkText, string actionName)
        {
            return ActionLinkSpan(helper, linkText, actionName, null, null);
        }

        public static MvcHtmlString ActionLinkSpan(this HtmlHelper helper, string linkText, string actionName, object routeValues)
        {
            return ActionLinkSpan(helper, linkText, actionName, routeValues, null);
        }

        public static MvcHtmlString ActionLinkSpan(this HtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            TagBuilder spanBuilder = new TagBuilder("span");
            spanBuilder.InnerHtml = linkText;

            TagBuilder aBuilder = new TagBuilder("a");
            aBuilder.InnerHtml = spanBuilder.ToString();
            aBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            aBuilder.MergeAttribute("href", url.Action(actionName, routeValues));

            return aBuilder.ToMvcHtmlString();
        }       
    }
}
