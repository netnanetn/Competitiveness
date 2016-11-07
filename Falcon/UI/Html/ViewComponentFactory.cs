using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public class ViewComponentFactory
    {
        public ViewComponentFactory(HtmlHelper helper)
        {
            HtmlHelper = helper;
        }

        public HtmlHelper HtmlHelper { get; set; }

        private ViewContext ViewContext
        {
            get
            {
                return HtmlHelper.ViewContext;
            }
        }

        public virtual TabBuilder Tab()
        {
            return new TabBuilder(new Tab(ViewContext));
        }

        public virtual SubMenuBuilder SubMenu()
        {
            return new SubMenuBuilder(new SubMenu(ViewContext));
        }
    }
}
