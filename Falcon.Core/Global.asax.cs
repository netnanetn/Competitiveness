using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Hosting;

using Falcon.Themes;
//using Falcon.EmbeddedViews;
using Falcon.Infrastructure;
using Falcon.Mvc.Routes;
using Falcon.Mvc;
using StackExchange.Profiling;
using System.Configuration;

using Common.Logging;
using System.Diagnostics;
using StackExchange.Profiling.EntityFramework6;

namespace Falcon.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class FalconFrameworkApplication : System.Web.HttpApplication
    {
        private ILog log = LogManager.GetCurrentClassLogger();

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandledErrorLoggerFilter());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterIgnoreRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("Thumbnail/{*pathInfo}");
            routes.IgnoreRoute("Upload/{*pathInfo}");
            routes.IgnoreRoute("Themes/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{handler}.ashx/{*pathInfo}");
            routes.IgnoreRoute("{*images}", new { images = @".*\.(jpg|jpeg|gif|png)(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Default",
                "",
                new { area = "Home", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Falcon.Modules.Home.Controllers" }
            );

            routes.MapRoute(
                "Admin", // Route name
                "Admin", // URL with parameters
                new { area = "CoreModules", controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                new string[] { "Falcon.Admin.CoreModules.Controllers" }
            );

            routes.MapRoute(
                "catchall", // Route name
                "{*path}", // URL with parameters
                new { area = "Contents", controller = "Pages", action = "Error" },
                new string[] { "Falcon.Modules.Contents.Controllers" }
            );
        }

        protected void Application_Start()
        {            
            log.Info("Start Application");

            var stopwatch = new Stopwatch();         
            stopwatch.Start();

            // disable the X-AspNetMvc-Version: header
            MvcHandler.DisableMvcResponseHeader = true;

            //initialize engine context
            EngineContext.Initialize(false);

            //set dependency resolver
            var dependencyResolver = new FalconDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);

            //model binders
            ModelBinders.Binders.Add(typeof(BaseModel), new FalconModelBinder());

            //MvcMiniprofiler
            MiniProfiler.Settings.IgnoredPaths = new string[] { "/upload/", "/mini-profiler-", "themes", "thumbnail", "/content/", "/scripts/", "/favicon.ico", "/asset.axd" };
            MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

            //remove all view engines
            System.Web.Mvc.ViewEngines.Engines.Clear();
            //except the themeable razor view engine we use                        
            //System.Web.Mvc.ViewEngines.Engines.Add(new ThemableRazorViewEngine());            
            System.Web.Mvc.ViewEngines.Engines.Add(new ProfilingViewEngine());

            //Add some functionality on top of the deafult ModelMetadataProvider
            ModelMetadataProviders.Current = new FalconMetadataProvider();

            //Registering some regular mvc stuf
            var stopwatch2 = new Stopwatch();
            stopwatch2.Start();

            RegisterIgnoreRoutes(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            stopwatch2.Stop();
            log.Info("Register Routes cost " + stopwatch2.Elapsed);

            GlobalFilters.Filters.Add(new ProfilingActionFilter());
            RegisterGlobalFilters(GlobalFilters.Filters);

            //For debugging
            // Read RouteDebugEnable setting in web.config file
            //string routeDebugEnable = ConfigurationManager.AppSettings["RouteDebugEnabled"];
            //if (routeDebugEnable == "true")
            //{
            //    RouteDebug.PreApplicationStart.Start();
            //}

            DataAnnotationsModelValidatorProvider
                .AddImplicitRequiredAttributeForValueTypes = false;
            //DefaultModelBinder.ResourceClassKey = "ValidateMessages";

            //ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new FalconValidatorFactory()));

            //register virtual path provider for embedded views
            //var embeddedViewResolver = EngineContext.Current.Resolve<IEmbeddedViewResolver>();
            //var embeddedProvider = new EmbeddedViewVirtualPathProvider(embeddedViewResolver.GetEmbeddedViews());
            //HostingEnvironment.RegisterVirtualPathProvider(embeddedProvider);

            MiniProfilerEF6.Initialize();

            log.Info("Start Application Completed, cost " + stopwatch.Elapsed);
        }

        protected void Application_BeginRequest()
        {
            if (Request.Cookies["MiniProfilerKey"] != null)
            {
                if (Request.Cookies["MiniProfilerKey"].Value.Equals(ConfigurationManager.AppSettings["MiniProfilerKey"]))
                {
                    MiniProfiler.Start();
                }
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}