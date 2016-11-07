using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Falcon.UI.Html
{
    public class TabItem
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ContentUrl { get; set; }
        public bool Visible { get; set; }
        public bool Enable { get; set; }
    }
}
