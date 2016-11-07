using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

using Falcon.Infrastructure;
using Falcon.Configuration;

namespace Falcon.UI.Html
{
    public static class ControllerExtensions
    {
        public static string ActionLink(this Controller c, string action, string controller, string area, object routeValues = null)
        {
            RouteValueDictionary route;

            if (routeValues != null)
            {
                /*
                 * Có thể dùng đoạn code này để lấy properties của 1 object
                 * route = new RouteValueDictionary();
                 * foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(routeValues))
                 * {
                 *   object obj2 = descriptor.GetValue(routeValues);
                 *   route.Add(descriptor.Name, obj2);
                 * }                                 
                 */
                route = new RouteValueDictionary(routeValues);
            }
            else
            {
                route = new RouteValueDictionary();
            }
            route.Add("action", action);
            route.Add("controller", controller);
            route.Add("area", area);
            //FalconConfig config = EngineContext.Current.Resolve<FalconConfig>();

            //return config.DomainName + RouteTable.Routes.GetVirtualPathForArea(c.ControllerContext.RequestContext, route).VirtualPath;
            return RouteTable.Routes.GetVirtualPathForArea(c.ControllerContext.RequestContext, route).VirtualPath;
        }
        public static string ActionLink(this Controller c, string action, string controller, object routeValues = null)
        {
            RouteValueDictionary route;

            if (routeValues != null)
            {
                route = new RouteValueDictionary(routeValues);
            }
            else
            {
                route = new RouteValueDictionary();
            }
            route.Add("action", action);
            route.Add("controller", controller);

            //FalconConfig config = EngineContext.Current.Resolve<FalconConfig>();

            //return config.DomainName + RouteTable.Routes.GetVirtualPath(c.ControllerContext.RequestContext, route).VirtualPath;
            return RouteTable.Routes.GetVirtualPathForArea(c.ControllerContext.RequestContext, route).VirtualPath;
        }
    }
}
