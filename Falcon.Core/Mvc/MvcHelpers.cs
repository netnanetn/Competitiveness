/// <copyright file="MvcHelpers.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace Falcon.Mvc
{
    public static class MvcHelpers
    {
        public static string GetAreaName(RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            // Trường hợp mặc định ở trang chủ, chạy qua default route
            // trong DataTokens ko có trường area
            if (!String.IsNullOrEmpty(routeData.Values["area"].ToString()))
            {
                return routeData.Values["area"].ToString();
            }
            return GetAreaName(routeData.Route);
        }

        public static string GetControllerName(RouteData routeData)
        {
            return routeData.Values["controller"].ToString();
        }

        public static string GetActionName(RouteData routeData)
        {
            return routeData.Values["action"].ToString();
        }

        private static string GetAreaName(RouteBase route)
        {
            var area = route as IRouteWithArea;
            if (area != null)
            {
                return area.Area;
            }
            var route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string);
            }
            return null;
        }
    }
}
