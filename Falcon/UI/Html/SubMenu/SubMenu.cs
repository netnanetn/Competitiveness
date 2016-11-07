using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public class SubMenu
    {
        public SubMenu(ViewContext viewContext)
        {
            ViewContext = viewContext;
            Items = new List<SubMenuItem>();
        }

        public string Key { get; set; }

        public string Css { get; set; }

        public ViewContext ViewContext { get; set; }

        public List<SubMenuItem> Items { get; set; }

        public MvcHtmlString Render()
        {
            string activeLink = "";
            object value = this.ViewContext.RouteData.Values[Key].ToString();
            if (value != null)
            {
                activeLink = value.ToString();
            }

            //Thẻ ul bao ngoài
            TagBuilder ul = new TagBuilder("ul");

            ul.AddCssClass(Css);

            //danh sách link            
            foreach (SubMenuItem item in Items)
            {
                TagBuilder liTag = new TagBuilder("li");
                
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("title", item.Name);
                aTag.MergeAttribute("href", item.Link);
                if (activeLink == item.Id)
                {
                    aTag.AddCssClass("active");
                }

                if (!string.IsNullOrEmpty(item.Icon))
                {
                    TagBuilder imgTag = new TagBuilder("img");
                    imgTag.MergeAttribute("src", item.Icon);
                    imgTag.MergeAttribute("alt", "icon");
                    imgTag.MergeAttribute("title", item.Name);

                    aTag.InnerHtml = item.Name + imgTag.ToString();
                }
                else
                {
                    aTag.SetInnerText(item.Name);
                }

                liTag.InnerHtml = aTag.ToString();

                ul.InnerHtml += liTag.ToString();
            }

            return ul.ToMvcHtmlString();
        }
    }
}
