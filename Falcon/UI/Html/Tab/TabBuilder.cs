using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public class TabBuilder
    {
        private Tab _tab;

        public TabBuilder(Tab tab)
        {
            _tab = tab;
        }

        public TabBuilder Title(string title)
        {
            _tab.Title = title;
            return this;
        }

        public TabBuilder Items(Action<TabItemBuilder> addAction)
        {
            TabItemBuilder builder = new TabItemBuilder(_tab.Items, _tab.ViewContext);

            addAction(builder);

            return this;
        }

        public TabBuilder Toolbar(string toolbar)
        {
            _tab.Toolbar = toolbar;
            return this;
        }

        public MvcHtmlString Render()
        {
            return _tab.Render();
        }
    }
}
