using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.Mvc.Controllers
{
    public static class PartialRequestsExtensions
    {
        public static void RenderPartialRequest(this HtmlHelper html, PartialRequest partial)
        {            
            if (partial != null)
                partial.Invoke(html.ViewContext);
        }

        public static void RenderPartialRequest(this HtmlHelper html, string actionName, string controllerName, string areaName)
        {
            var routeValues = new { action = actionName, controller = controllerName, area = areaName };
            RenderPartialRequest(html, routeValues);
        }

        public static void RenderPartialRequest(this HtmlHelper html, object routeValues)
        {
            PartialRequest partial = new PartialRequest(routeValues);
            if (partial != null)
                partial.Invoke(html.ViewContext);
        }
    }
}
