/// <copyright file="ThemeAttribute.cs" company="DKT">
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
    public class ThemeAttribute : ActionFilterAttribute
    {
        private readonly ThemeType _type;

        public ThemeAttribute(ThemeType type)
        {
            _type = type;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            IThemeContext themeContext = EngineContext.Current.Resolve<IThemeContext>();
            themeContext.CurrentThemeType = _type;
        }
    }

}
