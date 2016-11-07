using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Linq.Expressions;

namespace Falcon.UI.Html.Admin
{
    public static class ToolbarExtensions
    {
        public static MvcHtmlString BeginToolbar(this HtmlHelper helper, string title, string css = "")
        {
            return MvcHtmlString.Create("<div class='content-header'><table cellspacing='0'><tr><td style='width:50%;'><h3 class='" + css + "'>" + title + "</h3></td><td class='form-buttons'>");
        }

        public static MvcHtmlString EndToolbar(this HtmlHelper helper)
        {
            return MvcHtmlString.Create("</td></tr></table></div>");
        }
    }
}
