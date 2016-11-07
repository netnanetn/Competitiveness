using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Falcon.UI.Html.Admin
{
    public static class ButtonExtensions
    {
        public static MvcHtmlString Button(this HtmlHelper helper, string title)
        {
            return CreateButton(title);
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string title, object htmlAttributes)
        {
            return CreateButton(title, ButtonActionType.Submit, htmlAttributes);
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string title, ButtonActionType actionType)
        {
            return CreateButton(title, actionType);
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string title, ButtonActionType actionType, object htmlAttributes)
        {
            return CreateButton(title, actionType, htmlAttributes);
        }

        internal static MvcHtmlString CreateButton(string title, ButtonActionType actionType = ButtonActionType.Submit, object htmlAttributes = null)
        {
            var builder = new TagBuilder("button");

            builder.MergeAttribute("name", "submit");
            builder.MergeAttribute("value", actionType.ToString());
            builder.AddCssClass("scalable " + actionType.ToString());
            builder.MergeAttribute("type", "submit"); //want to use other types? use htmlAttributes to overrload

            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            var spanBuilder = new TagBuilder("span");
            spanBuilder.SetInnerText(title);
            builder.InnerHtml = spanBuilder.ToString();

            // Render button tag
            return builder.ToMvcHtmlString();
        }
    }
}
