/// <copyright file="BaseController.cs" company="DKT">
/// Copyright (c) 2012 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Common.Logging;
using Falcon.Themes;
using Falcon.Configuration;
using Falcon.Infrastructure;
using Falcon.UI;
using Falcon.Data.Domain;
using StackExchange.Profiling;
using System.Reflection;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Net;
using Elmah;
using System.Web;
using System.IO.Compression;

namespace Falcon.Mvc.Controllers
{
    [Theme(ThemeType.Portal)]    
    public class BaseController : Controller
    {
        private static ILog logger = LogManager.GetCurrentClassLogger();
        protected ISystemSettingService systemSettingService;
        private IWorkContext _workContext;

        public BaseController()
        {
            this.systemSettingService = FalconEngine.Resolve<ISystemSettingService>();
            this._workContext = FalconEngine.Resolve<IWorkContext>();            
        }

        public FalconEngine FalconEngine
        {
            get
            {
                return EngineContext.Current;
            }
        }

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
                return FalconEngine.FalconConfig;
            }
        }

        public ILog Logger
        {
            get
            {
                return logger;
            }
        }

        public MiniProfiler Profiler
        {
            get
            {
                return MiniProfiler.Current;
            }
        }

        public string Title
        {
            get
            {
                return ViewBag.Title;
            }
            set
            {
                ViewBag.Title = value;
            }
        }

        public string MetaDescription
        {
            get
            {
                return ViewBag.MetaDescription;
            }
            set
            {
                ViewBag.MetaDescription = value;
            }
        }

        public string MetaKeyword
        {
            get
            {
                return ViewBag.MetaKeyword;
            }
            set
            {
                ViewBag.MetaKeyword = value;
            }
        }

        public string CanonicalLink
        {
            get
            {
                return ViewBag.CanonicalLink;
            }
            set
            {
                ViewBag.CanonicalLink = value;
            }
        }

        public string LayoutName
        {
            get
            {
                return ViewBag.LayoutName;
            }
            set
            {
                ViewBag.LayoutName = value;
            }
        }

        protected virtual bool Matches(string test, string pattern)
        {
            return Regex.IsMatch(test, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // only process for main action, not for Child Action
            if (!filterContext.IsChildAction)
            {
                //string ip = GetRemoteIP();
                //check licence when application run online
                //string serverName = GetServerName();
                //if (EngineContext.Current.CheckLicense()) //IsPrivateIP(ip) || 
                //{
                    if (string.IsNullOrEmpty(Title))
                    {
                        //Set default Title for page
                        string area = MvcHelpers.GetAreaName(filterContext.RouteData);
                        string controller = MvcHelpers.GetControllerName(filterContext.RouteData);
                        string action = MvcHelpers.GetActionName(filterContext.RouteData);
                        //Set default Title for page
                        Title = action + " | " + controller;
                        if (area != "" && controller != area)
                        {
                            Title += " | " + area;
                        }
                    }

                    if (string.IsNullOrEmpty(MetaDescription))
                    {
                        MetaDescription = systemSettingService.Get("MetaDescription");
                    }

                    if (string.IsNullOrEmpty(MetaKeyword))
                    {
                        MetaKeyword = systemSettingService.Get("MetaKeyword");
                    }
                //}
                //else
                //{
                //    //Show Invalid Licence
                //    filterContext.Result = RedirectToAction("Invalid", new { area = "Users", controller = "Licence" });
                //}
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);            
            if (!string.IsNullOrEmpty(LayoutName))
            {
                var result = filterContext.Result as ViewResult;
                if (result != null)
                {
                    result.MasterName = LayoutName;
                    EngineContext.Current.Resolve<IThemeContext>().CurrentLayout = LayoutName;
                }
            }
        }

        /// <summary>
        /// Xử lý các trường hợp gọi đến phương không tồn tại hoặc tham số truyền không chính xác
        /// </summary>
        /// <param name="actionName"></param>
        protected override void HandleUnknownAction(string actionName)
        {
            HttpContext.Response.StatusCode = 404;
            HttpContext.Response.TrySkipIisCustomErrors = true;
            this.View("Error").ExecuteResult(this.ControllerContext);
        }

        protected virtual void WarningNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Warning, message, persistForTheNextRequest);
        }

        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true)
        {
            logger.Error(exception.Message);
            ErrorSignal.FromCurrentContext().Raise(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("falcon.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        protected virtual void AddModelStateErrors(bool persistForTheNextRequest = true)
        {
            foreach (ModelState modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    AddNotification(NotifyType.Error, error.ErrorMessage, persistForTheNextRequest);
                }
            }
        }

        /// <summary>
        /// Get all ModelState Errors
        /// </summary>
        /// <param name="htmlFormat">true -> use <br/>, false -> use /n</param>
        /// <returns></returns>
        protected virtual string GetModelStateErrors(bool htmlFormat = false)
        {
            string result = "";
            string separate = htmlFormat ? "<br/>" : "\n";
            foreach (ModelState modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    result +=  error.ErrorMessage + separate;
                }
            }
            return result;
        }

        /// <summary>
        /// Ghi log lỗi
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void RaiseError(Exception ex)
        {
            ErrorSignal.FromCurrentContext().Raise(ex);
        }

        /// <summary>
        /// Ghi log lỗi
        /// </summary>
        /// <param name="error"></param>
        protected virtual void RaiseError(string error)
        {
            ErrorSignal.FromCurrentContext().Raise(new Exception(error));
        }

        protected virtual ActionResult ShowErrorPage(string message = "")
        {
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            HttpContext.Response.TrySkipIisCustomErrors = true;
            ViewBag.ErrorMessage = message;
            return PartialView("~/Views/Shared/Error.cshtml");
        }

        /// <summary>
        /// When a client IP can't be determined
        /// </summary>
        public const string UnknownIP = "0.0.0.0";

        private static readonly Regex _ipAddress = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}$",
                                                             RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>
        /// returns true if this is a private network IP  
        /// http://en.wikipedia.org/wiki/Private_network
        /// </summary>
        protected static bool IsPrivateIP(string s)
        {
            return (s.StartsWith("192.168.") || s.StartsWith("10.") || s.StartsWith("127.0.0."));
        }

        /// <summary>
        /// retrieves the IP address of the current request -- handles proxies and private networks
        /// </summary>
        public static string GetRemoteIP(NameValueCollection ServerVariables)
        {
            string ip = ServerVariables["REMOTE_ADDR"]; // could be a proxy -- beware
            string ipForwarded = ServerVariables["HTTP_X_FORWARDED_FOR"];

            // check if we were forwarded from a proxy
            if (!string.IsNullOrEmpty(ipForwarded))
            {
                ipForwarded = _ipAddress.Match(ipForwarded).Value;
                if (!string.IsNullOrEmpty(ipForwarded) && !IsPrivateIP(ipForwarded))
                    ip = ipForwarded;
            }

            return !string.IsNullOrEmpty(ip) ? ip : UnknownIP;
        }

        /// <summary>
        /// Answers the current request's user's ip address; checks for any forwarding proxy
        /// </summary>
        public string GetRemoteIP()
        {
            return GetRemoteIP(Request.ServerVariables);
        }

        /// <summary>
        /// Get Server name using for this web application
        /// </summary>
        /// <returns></returns>
        protected string GetServerName()
        {
            return Request.ServerVariables["SERVER_NAME"];
        }

        private static readonly Regex _botUserAgent =
            new Regex(@"googlebot/\d|msnbot/\d|slurp/\d|jeeves/teoma|ia_archiver|ccbot/\d|yandex/\d|twiceler-\d",
                      RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// returns true if the current request is from a search engine, based on the User-Agent header
        /// </summary>
        protected bool IsSearchEngine()
        {
            if (string.IsNullOrEmpty(Request.UserAgent)) return false;
            return _botUserAgent.IsMatch(Request.UserAgent);
        }


        /// <summary>
        /// known good bot DNS lookups:  
        ///   66.249.68.73     crawl-66-249-68-73.googlebot.com  
        ///   66.235.124.58    crawler5107.ask.com  
        ///   65.55.104.157    msnbot-65-55-104-157.search.msn.com 
        /// </summary>
        private static readonly Regex _botDns = new Regex(@"(googlebot\.com|ask\.com|msn\.com)$",
                                                          RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture |
                                                          RegexOptions.Compiled);

        /// <summary>
        /// returns true if the current request is from a search engine, based on the User-Agent header *AND* a reverse DNS check
        /// </summary>
        protected bool IsSearchEngineDns()
        {
            if (!IsSearchEngine()) return false;
            string s = GetHostName();
            return _botDns.IsMatch(s);
        }

        /// <summary>
        /// perform a DNS lookup on the current IP address with a 2 second timeout
        /// </summary>
        /// <returns></returns>
        protected string GetHostName()
        {
            return GetHostName(GetRemoteIP(), 2000);
        }

        /// <summary>
        /// perform a DNS lookup on the provided IP address, with a timeout specified in milliseconds
        /// </summary>
        protected string GetHostName(string ipAddress, int timeout)
        {
            Func<string, string> fetcher = ip => Dns.GetHostEntry(ip).HostName;
            try
            {
                IAsyncResult result = fetcher.BeginInvoke(ipAddress, null, null);
                return result.AsyncWaitHandle.WaitOne(timeout, false) ? fetcher.EndInvoke(result) : "Timeout";
            }
            catch (Exception ex)
            {
                return ex.GetType().Name;
            }
        }
    }
}
