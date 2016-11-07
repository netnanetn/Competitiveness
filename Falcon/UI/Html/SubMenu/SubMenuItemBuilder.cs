using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public class SubMenuItemBuilder
    {
        private readonly List<SubMenuItem> _items;
        private readonly ViewContext _viewContext;

        public SubMenuItemBuilder(List<SubMenuItem> items, ViewContext viewContext)
        {
            _items = items;
            _viewContext = viewContext;
        }

        public SubMenuItemBuilder Add(SubMenuItem item)
        {
            _items.Add(item);
            return new SubMenuItemBuilder(_items, _viewContext);
        }

        public SubMenuItemBuilder Add(string name, string id, string link, string icon = "")
        {
            SubMenuItem item = new SubMenuItem() { Id = id, Name = name, Link = link, Icon = icon };
            _items.Add(item);
            return new SubMenuItemBuilder(_items, _viewContext);
        }        
    }
}
