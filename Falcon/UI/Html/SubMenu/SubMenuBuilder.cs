using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public class SubMenuBuilder
    {
        private SubMenu _subMenu;

        public SubMenuBuilder(SubMenu subMenu)
        {
            _subMenu = subMenu;
        }
        
        public SubMenuBuilder Key(string key)
        {
            _subMenu.Key = key;
            return this;
        }

        public SubMenuBuilder Css(string css)
        {
            _subMenu.Css = css;
            return this;
        }

        public SubMenuBuilder Items(Action<SubMenuItemBuilder> addAction)
        {
            SubMenuItemBuilder builder = new SubMenuItemBuilder(_subMenu.Items, _subMenu.ViewContext);

            addAction(builder);

            return this;
        }

        public MvcHtmlString Render()
        {
            return _subMenu.Render();
        }
    }
}
