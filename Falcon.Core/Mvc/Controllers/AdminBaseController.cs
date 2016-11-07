/// <copyright file="AdminBaseController.cs" company="DKT">
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

//using Falcon.Logging;
using Falcon.Themes;
using Falcon.Configuration;
using Falcon.Infrastructure;
using Falcon.Security;
using Falcon.Data.Domain;
using Falcon.UI;

namespace Falcon.Mvc.Controllers
{
    [Theme(ThemeType.Admin)]
    [Layout("Default")]
    public class AdminBaseController : BaseController
    {
        private IAuthenticationService _authenticationService;
        private IAuthorizationService _authorizationService;
        private IWorkContext _workContext;

        public AdminBaseController()
        {
            this._authenticationService = FalconEngine.Resolve<IAuthenticationService>();
            this._authorizationService = FalconEngine.Resolve<IAuthorizationService>();
            this._workContext = FalconEngine.Resolve<IWorkContext>();
        }

        #region Extra Properties for Admin Page
        public string HelpLink
        {
            get
            {
                return ViewBag.HelpLink;
            }
            set
            {
                ViewBag.HelpLink = value;
            }
        }

        public IWorkContext WorkContext
        {
            get
            {
                return _workContext;
            }
        }
        #endregion

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //set work context to admin mode
            WorkContext.IsAdmin = true;

            base.Initialize(requestContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                //validate IP address
                if (ValidateIpAddress())
                {
                    // check user login?
                    User user = WorkContext.CurrentUser;

                    if (user == null)
                    {
                        //Logger.Error("Access denied to anonymous user");
                        filterContext.Result = RedirectToAction("index", new { area = "CoreModules", controller = "Login", returnUrl = filterContext.HttpContext.Request.RawUrl });
                    }
                    else
                    {
                        // check permission?
                        string area = MvcHelpers.GetAreaName(filterContext.RouteData);
                        string controller = MvcHelpers.GetControllerName(filterContext.RouteData);
                        string action = MvcHelpers.GetActionName(filterContext.RouteData);

                        if (user.Id != 1 && !_authorizationService.isAllowed(user, area + "/" + controller, action))
                        {
                            //Logger.Error("Access denied to this user: " + filterContext.HttpContext.Request.QueryString["ReturnUrl"]);
                            filterContext.Result = RedirectToAction("AccessDenied", new { area = "Users", controller = "Users" });
                        }
                        else
                        {
                            //Set default Title for page
                            Title = action + " | " + controller;
                            if (area != "" && controller != area)
                            {
                                Title += " | " + area;
                            }
                        }
                    }
                }
                else
                {
                    filterContext.Result = RedirectToAction("AccessDenied", new { area = "Users", controller = "Users" });
                }
            }

            base.OnActionExecuting(filterContext);
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    if (filterContext.Exception != null)
        //        LogException(filterContext.Exception);
        //    base.OnException(filterContext);
        //}

        protected virtual bool ValidateIpAddress()
        {
            //string currentIp = GetRemoteIP();
            //bool isAllow = false;
            //if (IsPrivateIP(currentIp))
            //{
            //    isAllow = true;
            //}
            //else
            //{
            //    //check ip exist in Denied Directory
            //    if (!System.IO.File.Exists(Server.MapPath("/Ip/Banned/" + currentIp)))
            //    {
            //        string filePath = Server.MapPath("/Ip/Allowed/" + currentIp);
            //        if (System.IO.File.Exists(filePath))
            //        {
            //            var lastTouch = System.IO.File.GetLastWriteTime(filePath);
            //            //1 địa chỉ Ip chỉ được tồn tại trong 1 ngày, sau đó cần phải xác minh lại
            //            if (lastTouch.CompareTo(DateTime.Now.AddDays(-1)) > 0)
            //            {
            //                isAllow = true;
            //            }
            //        }
            //    }
            //}            
            //return isAllow;
            return true;
        }

        //public virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales) where TLocalizedModelLocal : ILocalizedModelLocal
        //{
        //    AddLocales(languageService, locales, null);
        //}
        //public virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales, Action<TLocalizedModelLocal, int> configure) where TLocalizedModelLocal : ILocalizedModelLocal
        //{
        //    foreach (var language in languageService.GetAllLanguages(true))
        //    {
        //        var locale = Activator.CreateInstance<TLocalizedModelLocal>();
        //        locale.LanguageId = language.Id;
        //        if (configure != null)
        //        {
        //            configure.Invoke(locale, locale.LanguageId);
        //        }
        //        locales.Add(locale);
        //    }
        //}

        /// <summary>
        /// Access denied view
        /// </summary>
        /// <returns>Access denied view</returns>
        protected ActionResult AccessDeniedView()
        {
            //return new HttpUnauthorizedResult();
            return RedirectToAction("AccessDenied", "Users", new { pageUrl = this.Request.RawUrl });
        }


        //private void LogException(Exception exc)
        //{
        //    var workContext = FalconEngine.Resolve<IWorkContext>();
        //    var logger = FalconEngine.Resolve<ILogger>();

        //    var customer = workContext.CurrentCustomer;
        //    logger.Error(exc.Message, exc, customer);
        //}

        //protected virtual void WarningNotification(string message, bool persistForTheNextRequest = true)
        //{
        //    AddNotification(NotifyType.Warning, message, persistForTheNextRequest);
        //}

        //protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        //{
        //    AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        //}
        //protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        //{
        //    AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        //}
        //protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true)
        //{
        //    Logger.Error(exception.Message);
        //    AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        //}
        //protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        //{
        //    string dataKey = string.Format("falcon.notifications.{0}", type);
        //    if (persistForTheNextRequest)
        //    {
        //        if (TempData[dataKey] == null)
        //            TempData[dataKey] = new List<string>();
        //        ((List<string>)TempData[dataKey]).Add(message);
        //    }
        //    else
        //    {
        //        if (ViewData[dataKey] == null)
        //            ViewData[dataKey] = new List<string>();
        //        ((List<string>)ViewData[dataKey]).Add(message);
        //    }
        //}

        //protected virtual void AddModelStateErrors(bool persistForTheNextRequest = true)
        //{
        //    foreach (ModelState modelState in ModelState.Values)
        //    {
        //        foreach (ModelError error in modelState.Errors)
        //        {
        //            AddNotification(NotifyType.Error, error.ErrorMessage, persistForTheNextRequest);
        //        }
        //    }
        //}
        
    }
}
