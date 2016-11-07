using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;

namespace Falcon.Mvc.Controllers
{
    public class PartialRequest
    {
        public RouteValueDictionary RouteValues { get; private set; }

        public PartialRequest(object routeValues)
        {
            RouteValues = new RouteValueDictionary(routeValues);
        }

        public void Invoke(ControllerContext context)
        {
            RouteData rd = new RouteData(context.RouteData.Route, context.RouteData.RouteHandler);
            foreach (var pair in RouteValues)
                rd.Values.Add(pair.Key, pair.Value);
            IHttpHandler handler = new MvcHandler(new RequestContext(context.HttpContext, rd));
            handler.ProcessRequest(HttpContext.Current);
        }
    }
}
