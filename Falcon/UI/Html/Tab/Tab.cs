using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.UI.Html
{
    public class Tab
    {
        public Tab(ViewContext viewContext)
        {
            ViewContext = viewContext;
            Items = new List<TabItem>();
        }

        public ViewContext ViewContext { get; set; }

        public string Title { get; set; }

        public List<TabItem> Items { get; set; }

        public string Toolbar { get; set; }

        public MvcHtmlString Render()
        {
            //Thẻ div bao ngoài
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("columns");

            //Thẻ div bên trái
            TagBuilder divLeft = new TagBuilder("div");
            divLeft.AddCssClass("side-col");
            divLeft.MergeAttribute("id", "page:left");
            
            //tiêu đề
            TagBuilder titleTag = new TagBuilder("h3");
            titleTag.SetInnerText(Title);

            //danh sách tabs
            TagBuilder tabs = new TagBuilder("ul");
            tabs.AddCssClass("tabs");
            tabs.MergeAttribute("id", "page_tabs");
            foreach (TabItem item in Items)
            {
                TagBuilder liTag = new TagBuilder("li");
                
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("id", "page_tabs_" + item.Name + "_section");
                aTag.AddCssClass("tab-item-link");
                aTag.MergeAttribute("title", item.Title);
                aTag.MergeAttribute("name", "main_" + item.Name);
                aTag.MergeAttribute("href", "#");

                TagBuilder spanTag = new TagBuilder("span");
                spanTag.SetInnerText(item.Title);

                aTag.InnerHtml = spanTag.ToString();

                liTag.InnerHtml = aTag.ToString();

                tabs.InnerHtml += liTag.ToString();
            }

            divLeft.InnerHtml = titleTag.ToString();
            divLeft.InnerHtml += tabs.ToString();

            //thẻ div nội dung
            TagBuilder divContent = new TagBuilder("div");
            divContent.AddCssClass("main-col");
            divContent.MergeAttribute("id", "content");

            TagBuilder divInnerContent = new TagBuilder("div");
            divInnerContent.AddCssClass("main-col-inner");

            divInnerContent.InnerHtml = Toolbar;

            foreach (TabItem item in Items)
            {
                TagBuilder divTag = new TagBuilder("div");

                divTag.MergeAttribute("id", "page_tabs_" + item.Name + "_section_content");
                divTag.InnerHtml = item.Content;

                divInnerContent.InnerHtml += divTag.ToString();
            }
            divContent.InnerHtml = divInnerContent.ToString();

            TagBuilder scriptTag = new TagBuilder("script");
            scriptTag.MergeAttribute("type", "text/javascript");
            scriptTag.InnerHtml = "jQuery('#page_tabs').falconTabs({activeTabId: 'page_tabs_" + Items[0].Name + "_section'});";

            div.InnerHtml = divLeft.ToString() + divContent.ToString() + scriptTag.ToString();



            return div.ToMvcHtmlString();
        }
    }
}
