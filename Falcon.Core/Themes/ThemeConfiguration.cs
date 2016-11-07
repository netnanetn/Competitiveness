using System.Xml;
using System.Collections.Generic;

namespace Falcon.Themes
{
    public class ThemeConfiguration
    {        
        public ThemeConfiguration(string themeName, string path, string virtualPath, XmlDocument doc)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", "http://tempuri.org/Theme.xsd");

            ThemeName = themeName;
            Path = path;
            VirtualPath = virtualPath;
            XmlNode node = doc.SelectSingleNode("/ns:Theme", nsmgr);
            if (node != null)
            {
                ConfigurationNode = node;
                var attribute = node.Attributes["Title"];
                ThemeTitle = attribute == null ? string.Empty : attribute.Value;
                
                attribute = node.Attributes["Image"];
                Image = attribute == null ? string.Empty : attribute.Value;

                attribute = node.Attributes["ImageThumb"];
                ImageThumb = attribute == null ? string.Empty : attribute.Value;

            }
        }

        public XmlNode ConfigurationNode { get; protected set; }

        public string Path { get; protected set; }

        public string VirtualPath { get; protected set; }        

        public string ThemeName { get; protected set; }

        public string ThemeTitle { get; protected set; }

        public string Image { get; protected set; }

        public string ImageThumb { get; protected set; }
    }
}
