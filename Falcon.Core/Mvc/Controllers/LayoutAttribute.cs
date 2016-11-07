/// <copyright file="LayoutAttribute.cs" company="DKT">
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

using Falcon.Infrastructure;
using Falcon.Themes;

namespace Falcon.Mvc.Controllers
{
    public class LayoutAttribute : ActionFilterAttribute
    {
        private readonly string _masterName;
        public LayoutAttribute(string masterName)
        {
            _masterName = masterName;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;
            if (result != null)
            {
                result.MasterName = _masterName;
                EngineContext.Current.Resolve<IThemeContext>().CurrentLayout = _masterName;
            }
        }
    }

}
