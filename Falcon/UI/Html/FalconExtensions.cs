using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public static class FalconExtensions
    {
        public static ViewComponentFactory Falcon(this HtmlHelper helper)
        {
            return new ViewComponentFactory(helper);
        }
    }
}
