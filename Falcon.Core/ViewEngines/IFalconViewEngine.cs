using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon;
using System.Web.Mvc;

namespace Falcon.ViewEngines
{
    interface IFalconViewEngine
    {
        IWorkContext WorkContext {get; }

        //MvcHtmlString RenderZone(string zoneName);

    }
}
