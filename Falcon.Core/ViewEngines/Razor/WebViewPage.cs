/// <copyright file="WebViewPage.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using System.Configuration;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;

using Falcon.Infrastructure;
//using Falcon.Localization;
using Falcon.Themes;
using Falcon.Configuration;
using Falcon.UI.Resources;
using Falcon.Data.Domain;

namespace Falcon.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>, IFalconViewEngine
    {

        //private ILocalizationService _localizationService;
        private ISystemSettingService _systemSettingService;
        //private Localizer _localizer;
        private IWorkContext _workContext;
        private string _absoluteThemePath;
        private string _themeViewPath;
        private IThemeContext _themeContext;
        private ThemeConfiguration _themeConfiguration;

        //public Localizer T
        //{
        //    get
        //    {
        //        if (_localizer == null)
        //        {
        //            null localizer
        //            _localizer = (format, args) => new LocalizedString((args == null || args.Length == 0) ? format : string.Format(format, args));

        //            default localizer
        //            _localizer = (format, args) =>
        //                             {
        //                                 var resFormat = _localizationService.GetResource(format);
        //                                 if (string.IsNullOrEmpty(resFormat))
        //                                 {
        //                                     return new LocalizedString(format);
        //                                 }
        //                                 return
        //                                     new LocalizedString((args == null || args.Length == 0)
        //                                                             ? resFormat
        //                                                             : string.Format(resFormat, args));
        //                             };
        //        }
        //        return _localizer;
        //    }
        //}

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext;
            }
        }

        public FalconConfig FalconConfig
        {
            get
            {
                return EngineContext.Current.FalconConfig;
            }
        }

        /// <summary>
        /// Get Absolute Theme Path for client usage (example: /Themes/Portal/... instead of ~/Themes/Portal/...)
        /// </summary>
        public string AbsoluteThemePath
        {
            get
            {
                if (_absoluteThemePath == null)
                {
                    _absoluteThemePath = System.Web.VirtualPathUtility.ToAbsolute(_themeContext.CurrentThemePath);
                }
                return _absoluteThemePath;
            }
        }

        public string ThemeViewPath
        {
            get
            {
                if (_themeViewPath == null)
                {
                    _themeViewPath = _themeContext.CurrentThemePath + "Views/";
                }
                return _themeViewPath;
            }
        }

        public ThemeConfiguration ThemeConfiguration
        {
            get
            {
                return _themeConfiguration;
            }
        }

        public ISystemSettingService SystemSettingService
        {
            get
            {
                if (_systemSettingService == null)
                {
                    _systemSettingService = EngineContext.Current.Resolve<ISystemSettingService>();
                }
                return _systemSettingService;
            }
        }

        public override void InitHelpers()
        {
            base.InitHelpers();
            //_localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            _workContext = EngineContext.Current.Resolve<IWorkContext>();
            _systemSettingService = EngineContext.Current.Resolve<ISystemSettingService>();

            _themeContext = EngineContext.Current.Resolve<IThemeContext>();
            _themeConfiguration = _themeContext.CurrentThemeConfiguration;
        }

        public HelperResult RenderWrappedSection(string name, object wrapperHtmlAttributes)
        {
            Action<TextWriter> action = delegate(TextWriter tw)
                                {
                                    var htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(wrapperHtmlAttributes);
                                    var tagBuilder = new TagBuilder("div");
                                    tagBuilder.MergeAttributes(htmlAttributes);

                                    var section = RenderSection(name, false);
                                    if (section != null)
                                    {
                                        tw.Write(tagBuilder.ToString(TagRenderMode.StartTag));
                                        section.WriteTo(tw);
                                        tw.Write(tagBuilder.ToString(TagRenderMode.EndTag));
                                    }
                                };
            return new HelperResult(action);
        }

        public HelperResult RenderSection(string sectionName, Func<object, HelperResult> defaultContent)
        {
            return IsSectionDefined(sectionName) ? RenderSection(sectionName) : defaultContent(new object());
        }

        /// <summary>
        /// Create html link tag (a tag)
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public MvcHtmlString RegisterLink(LinkEntry link)
        {
            return new MvcHtmlString(link.GetTag());
        }

        /// <summary>
        /// Set meta data for page
        /// </summary>
        /// <param name="metaEntry"></param>
        /// <returns></returns>
        public MvcHtmlString SetMeta(MetaEntry metaEntry)
        {
            return new MvcHtmlString(metaEntry.GetTag());
        }

        /// <summary>
        /// Set meta data for page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public MvcHtmlString SetMeta(string name, string content)
        {
            return new MvcHtmlString(new MetaEntry { Name = name, Content = content }.GetTag());
        }

        /// <summary>
        /// Include javascript library for page
        /// </summary>
        /// <param name="scriptPath">path to the javascript file</param>
        /// <param name="useThemeScriptPath">true: use script in the current theme folder, 
        /// false: use script in the general Scripts folder</param>
        /// <returns></returns>
        public MvcHtmlString Js(string scriptPath, bool useThemeScriptPath = false)
        {
            var builder = new TagBuilder("script");
            builder.MergeAttribute("type", "text/javascript");
            if (useThemeScriptPath)
            {
                builder.MergeAttribute("src", FalconConfig.StaticDomainName + AbsoluteThemePath + "Scripts/" + scriptPath);
            }
            else
            {
                builder.MergeAttribute("src", FalconConfig.StaticDomainName + "/Scripts/" + scriptPath);
            }
            
            return new MvcHtmlString(builder.ToString());
        }

        /// <summary>
        /// Include stylesheet for page
        /// </summary>
        /// <param name="cssPath">path to the css file</param>
        /// <param name="htmlAttributes">additional options for this css</param>
        /// <returns></returns>
        public MvcHtmlString Css(string cssPath, object htmlAttributes = null)
        {
            var builder = new TagBuilder("link");            
            builder.MergeAttribute("rel", "stylesheet");
            builder.MergeAttribute("type", "text/css");
            builder.MergeAttribute("href", FalconConfig.StaticDomainName + AbsoluteThemePath + "Styles/" + cssPath);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Generate image tag
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="htmlAttributes">additional options for this image</param>
        /// <returns></returns>
        public MvcHtmlString Img(string imagePath, object htmlAttributes = null)
        {
            // Create tag builder
            var builder = new TagBuilder("img");

            // Create valid id
            //builder.GenerateId(id);

            // Add attributes
            builder.MergeAttribute("src", FalconConfig.MediaDomainName + AbsoluteThemePath + "Images/" + imagePath);            
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // Render tag
            var img = builder.ToString(TagRenderMode.SelfClosing);

            return new MvcHtmlString(img);
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
