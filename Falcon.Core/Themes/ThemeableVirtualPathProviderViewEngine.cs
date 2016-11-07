using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Falcon.Infrastructure;
using Common.Logging;

namespace Falcon.Themes
{
    public abstract class ThemeableVirtualPathProviderViewEngine : VirtualPathProviderViewEngine
    {
        //private ILog logger = LogManager.GetCurrentClassLogger();

        internal Func<string, string> GetExtensionThunk;

        private static readonly string[] _emptyLocations;

        protected ThemeableVirtualPathProviderViewEngine()
        {
            GetExtensionThunk = new Func<string, string>(VirtualPathUtility.GetExtension);
        }

        protected string GetViewPath(ControllerContext controllerContext, string[] locations, string viewName, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;
            if (string.IsNullOrEmpty(viewName))
            {
                return string.Empty;
            }
            string areaName = GetAreaName(controllerContext.RouteData);

            bool flag = !string.IsNullOrEmpty(areaName);
            List<ViewLocation> viewLocations = GetViewLocations(locations);
            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Properties cannot be null or empty.", new object[] { "ViewLocationFormats" }));
            }
            bool flag2 = IsSpecificPath(viewName);
            string key = this.CreateCacheKey(cacheKeyPrefix, viewName, flag2 ? string.Empty : controllerName, areaName, string.Empty);
            if (useCache)
            {
                var cached = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (cached != null)
                {
                    return cached;
                }
            }
            if (!flag2)
            {
                return this.GetViewPathFromGeneralName(controllerContext, viewLocations, viewName, controllerName, areaName, key, ref searchedLocations);
            }
            return this.GetPathFromSpecificName(controllerContext, viewName, key, ref searchedLocations);
        }

        private string GetMasterPath(ControllerContext controllerContext, string[] locations, string masterName, string theme, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;
            if (string.IsNullOrEmpty(masterName))
            {
                return string.Empty;
            }
            
            //bool flag = !string.IsNullOrEmpty(areaName);
            List<ViewLocation> viewLocations = GetViewLocations(locations);
            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Properties cannot be null or empty.", new object[] { "MasterLocationFormats" }));
            }
            bool flag2 = IsSpecificPath(masterName);
            string key = this.CreateCacheKey(cacheKeyPrefix, masterName, string.Empty, string.Empty, theme);
            if (useCache)
            {
                var cached = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (cached != null)
                {
                    return cached;
                }
            }
            if (!flag2)
            {
                return this.GetMasterPathFromGeneralName(controllerContext, viewLocations, masterName, theme, key, ref searchedLocations);
            }
            return this.GetPathFromSpecificName(controllerContext, masterName, key, ref searchedLocations);
        }

        private bool FilePathIsSupported(string virtualPath)
        {
            if (this.FileExtensions == null)
            {
                return true;
            }
            string str = this.GetExtensionThunk(virtualPath).TrimStart(new char[] { '.' });
            return this.FileExtensions.Contains<string>(str, StringComparer.OrdinalIgnoreCase);
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = name;
            if (!this.FilePathIsSupported(name) || !this.FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new string[] { name };
            }
            this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }        

        private string GetViewPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string viewName, string controllerName, string areaName, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = string.Empty;
            searchedLocations = new string[locations.Count];
            for (int i = 0; i < locations.Count; i++)
            {
                string str2 = locations[i].Format(viewName, controllerName, areaName);
                if (this.FileExists(controllerContext, str2))
                {
                    searchedLocations = _emptyLocations;
                    virtualPath = str2;
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = str2;
            }
            return virtualPath;
        }

        private string GetMasterPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string masterName, string theme, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = string.Empty;
            searchedLocations = new string[locations.Count];
            for (int i = 0; i < locations.Count; i++)
            {
                string str2 = locations[i].MasterFormat(masterName, theme);
                if (this.FileExists(controllerContext, str2))
                {
                    searchedLocations = _emptyLocations;
                    virtualPath = str2;
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = str2;
            }
            return virtualPath;
        }

        private string CreateCacheKey(string prefix, string name, string controllerName, string areaName, string theme)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:{5}", new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName, theme });
        }

        public static string GetAreaName(RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            //KhoiNM: Trường hợp mặc định ở trang chủ, chạy qua default route
            // trong DataTokens ko có trường area
            if (!String.IsNullOrEmpty(routeData.Values["area"].ToString()))
            {
                return routeData.Values["area"].ToString();
            }
            return GetAreaName(routeData.Route);
        }

        public static string GetAreaName(RouteBase route)
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

        private static List<ViewLocation> GetViewLocations(string[] viewLocationFormats)
        {
            var list = new List<ViewLocation>();
            
            if (viewLocationFormats != null)
            {
                list.AddRange(viewLocationFormats.Select(str => new ViewLocation(str)));
            }
            return list;
        }

        private static bool IsSpecificPath(string name)
        {
            char ch = name[0];
            if (ch != '~')
            {
                return (ch == '/');
            }
            return true;
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            
            string[] searchedViewLocations;
            string[] searchedMasterLocations;
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("View name cannot be null or empty.", "viewName");
            }
            
            IThemeContext themeContext = EngineContext.Current.Resolve<IThemeContext>();
            var theme = themeContext.CurrentTheme;
            
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            

            string viewPath = this.GetViewPath(controllerContext, this.AreaViewLocationFormats, viewName, requiredString, "View", useCache, out searchedViewLocations);

            string masterPath;
            masterPath = this.GetMasterPath(controllerContext, this.MasterLocationFormats, masterName, theme, "Master", useCache, out searchedMasterLocations);

            //if (isLogRequest && !string.IsNullOrEmpty(masterPath))
            //{
            //    logger.Debug(string.Format("request:{0},theme:{1},isStoreTheme:{2},controller:{3},viewPath:{4},masterPath:{5}", rawUrl, theme, themeContext.IsStoreTheme, requiredString, viewPath, masterPath));
            //}
            
            if (!string.IsNullOrEmpty(viewPath) && (!string.IsNullOrEmpty(masterPath) || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(this.CreateView(controllerContext, viewPath, masterPath), this);
            }
            if(searchedMasterLocations == null)
            {
                searchedMasterLocations = new string[0];
            }
            
            return new ViewEngineResult(searchedViewLocations.Union<string>(searchedMasterLocations));
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            string[] strArray;
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Partial view name cannot be null or empty.", "partialViewName");
            }

            var themeContext = EngineContext.Current.Resolve<IThemeContext>();
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");

            string viewPath = this.GetViewPath(controllerContext, this.PartialViewLocationFormats, partialViewName, requiredString, "Partial", useCache, out strArray);
            if (string.IsNullOrEmpty(viewPath))
            {
                return new ViewEngineResult(strArray);
            }
            return new ViewEngineResult(this.CreatePartialView(controllerContext, viewPath), this);

        }
    }

    public class AreaAwareViewLocation : ViewLocation
    {
        public AreaAwareViewLocation(string virtualPathFormatString)
            : base(virtualPathFormatString)
        {
        }

        public override string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName);
        }

        public override string MasterFormat(string masterName, string theme)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, masterName, theme);
        }
    }

    public class ViewLocation
    {
        protected readonly string _virtualPathFormatString;

        public ViewLocation(string virtualPathFormatString)
        {
            _virtualPathFormatString = virtualPathFormatString;
        }

        public virtual string Format(string viewName, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName);
        }

        public virtual string MasterFormat(string masterName, string theme)
        {
            return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, masterName, theme);
        }
    }
}
