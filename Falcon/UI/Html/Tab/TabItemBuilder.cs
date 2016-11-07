using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public class TabItemBuilder
    {
        private readonly List<TabItem> _items;
        private readonly ViewContext _viewContext;

        public TabItemBuilder(List<TabItem> items, ViewContext viewContext)
        {
            _items = items;
            _viewContext = viewContext;
        }

        public TabItemBuilder Add(TabItem item)
        {
            _items.Add(item);
            return new TabItemBuilder(_items, _viewContext);
        }

        public TabItemBuilder Add(string name, string title, string content)
        {
            TabItem item = new TabItem() { Name = name, Title = title, Content = content};
            _items.Add(item);
            return new TabItemBuilder(_items, _viewContext);
        }

        public TabItemBuilder Add(string name, string title, MvcHtmlString content)
        {
            TabItem item = new TabItem() { Name = name, Title = title};
            item.Content = content.ToString();

            _items.Add(item);
            return new TabItemBuilder(_items, _viewContext);
        }

        public TabItemBuilder Add(string name, string title, Func<object, object> content)
        {
            TabItem item = new TabItem() { Name = name, Title = title };
            item.Content = content(item).ToString();

            _items.Add(item);
            return new TabItemBuilder(_items, _viewContext);
        }
    }
}
