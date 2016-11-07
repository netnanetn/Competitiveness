using System.Collections.Generic;
using System.Web.Mvc;
using Falcon.Configuration;
using Falcon.Infrastructure;

namespace Falcon.Themes
{
    public class ThemableRazorViewEngine : ThemeableBuildManagerViewEngine
    {
        public ThemableRazorViewEngine()
        {
            FalconConfig falconConfig = EngineContext.Current.Resolve<FalconConfig>();
            AreaViewLocationFormats = new[]
                                          {
                                              //default
                                              "~/Modules/{2}/Views/{1}/{0}.cshtml",
                                              "~/Views/Shared/{0}.cshtml",

                                              //Admin
                                              "~/AdminModules/{2}/Views/{1}/{0}.cshtml",
                                              "~/AdminModules/{2}/Views/Shared/{0}.cshtml",
                                          };

            //AreaMasterLocationFormats = new[]
            //                                {
            //                                    //default
            //                                    "~/Areas/{2}/Views/{1}/{0}.cshtml", 
            //                                    "~/Areas/{2}/Views/Shared/{0}.cshtml"
            //                                };

            AreaPartialViewLocationFormats = new[]
                                                 {
                                                    //default
                                                    "~/Modules/{2}/Views/{1}/{0}.cshtml",
                                                    "~/Views/Shared/{0}.cshtml",

                                                    //Admin
                                                    "~/AdminModules/{2}/Views/{1}/{0}.cshtml",
                                                    "~/AdminModules/{2}/Views/Shared/{0}.cshtml",
                                                 };

            //ViewLocationFormats = new[]
            //                          {
            //                                //themes
            //                                "~/Themes/{2}/Views/{1}/{0}.cshtml", 
            //                                "~/Themes/{2}/Views/Shared/{0}.cshtml",

            //                                //default
            //                                "~/Views/{1}/{0}.cshtml", 
            //                                "~/Views/Shared/{0}.cshtml"
            //                          };

            MasterLocationFormats = new[]
                                        {
                                            //themes
                                            falconConfig.ThemeBasePath + "{1}/Views/Layouts/{0}.cshtml", 
                                        };
            PartialViewLocationFormats = new[]
                                             {                                                
                                                "~/Modules/{2}/Views/{1}/{0}.cshtml",
                                                "~/Views/Shared/{0}.cshtml",

                                                //Admin
                                                "~/AdminModules/{2}/Views/{1}/{0}.cshtml",
                                                "~/AdminModules/{2}/Views/Shared/{0}.cshtml",

                                                //themes
                                                falconConfig.ThemeBasePath + "{2}/Views/{1}/{0}.cshtml", 
                                             };

            FileExtensions = new[] { "cshtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            var runViewStartPages = false;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;
            var runViewStartPages = false;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, layoutPath, runViewStartPages, fileExtensions);
        }
    }
}
