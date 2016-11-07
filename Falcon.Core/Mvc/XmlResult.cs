using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Falcon.Mvc
{
    public class XmlResult : ActionResult
    {
        public XmlResult(string xml)
        {
            Xml = new XmlDocument();
            Xml.LoadXml(xml);
        }

        public XmlResult(XmlDocument xml)
        {
            Xml = xml;
        }

        public XmlDocument Xml
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            XmlDeclaration decl = Xml.FirstChild as XmlDeclaration;
            if (decl != null)
            {
                decl.Encoding = "utf-8";
            }
            context.HttpContext.Response.Charset = "utf-8";
            context.HttpContext.Response.ContentType = "text/xml";
            context.HttpContext.Response.BinaryWrite(Encoding.UTF8.GetBytes(Xml.InnerXml));
            context.HttpContext.Response.End();
        }
    }
}
