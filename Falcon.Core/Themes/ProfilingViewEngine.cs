using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using StackExchange.Profiling;

namespace Falcon.Themes
{
    public class ProfilingViewEngine : ThemableRazorViewEngine
    {
        class WrappedView : IView
        {
            IView wrapped;
            string name;
            bool isPartial;

            public WrappedView(IView wrapped, string name, bool isPartial)
            {
                this.wrapped = wrapped;
                this.name = name;
                this.isPartial = isPartial;
            }

            public void Render(ViewContext viewContext, System.IO.TextWriter writer)
            {
                using (MiniProfiler.Current.Step("Render " + (isPartial ? "partial" : "") + ": " + name))
                {
                    wrapped.Render(viewContext, writer);
                }
            }
        }

        ThemableRazorViewEngine wrapped;

        public ProfilingViewEngine()
        {
            wrapped = new ThemableRazorViewEngine();
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var found = wrapped.FindPartialView(controllerContext, partialViewName, useCache);
            if (found != null && found.View != null)
            {
                found = new ViewEngineResult(new WrappedView(found.View, partialViewName, isPartial: true), this);
            }
            return found;
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var found = wrapped.FindView(controllerContext, viewName, masterName, useCache);
            if (found != null && found.View != null)
            {
                found = new ViewEngineResult(new WrappedView(found.View, viewName, isPartial: false), this);
            }
            return found;
        }

        public override void ReleaseView(ControllerContext controllerContext, IView view)
        {
            wrapped.ReleaseView(controllerContext, view);
        }

        public string FindViewPath(ControllerContext controllerContext, string controller, string viewName, bool useCache)
        {
            string[] searchedViewLocations;

            string viewPath = GetViewPath(controllerContext, AreaViewLocationFormats, viewName, controller, "View", useCache, out searchedViewLocations);

            return viewPath;
        }

        public string FindPartialViewPath(ControllerContext controllerContext, string controller, string partialViewName, bool useCache)
        {
            string[] strArray;

            string partialViewPath = GetViewPath(controllerContext, this.PartialViewLocationFormats, partialViewName, controller, "Partial", useCache, out strArray);

            return partialViewPath;
        }
    }
}
