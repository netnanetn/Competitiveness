/// <copyright file="ModuleConfiguration.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description>Mô tả module của FalconFramwork</description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Falcon.Security
{
    public class ModuleConfiguration
    {
        public ModuleConfiguration()
        {
            this.Controllers = new List<ControllerInfo>();
        }
        public ModuleConfiguration(string moduleName, string path, string virtualPath, XmlDocument doc)
        {
            Name = moduleName;
            Path = path;
            VirtualPath = virtualPath;
            Controllers = new List<ControllerInfo>();

            var mdlNode = doc.SelectSingleNode("module");
            if (mdlNode != null)
            {
                ConfigurationNode = mdlNode;

                var descNode = mdlNode.SelectSingleNode("description");
                Description = descNode == null ? string.Empty : descNode.InnerText;

                var ctlNode = mdlNode.SelectSingleNode("controllers");
                if (ctlNode != null)
                {                    
                    var ctlsNode = ctlNode.SelectNodes("controller");
                    foreach (XmlNode node in ctlsNode)
                    {
                        ControllerInfo controller = new ControllerInfo();
                        controller.Name = node.Attributes["name"] == null ? string.Empty : node.Attributes["name"].Value;
                        foreach (XmlNode action in node.SelectNodes("action"))
                        {
                            controller.Actions.Add(action.InnerText);
                        }

                        Controllers.Add(controller);
                    }
                }
            }
        }

        public XmlNode ConfigurationNode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string VirtualPath { get; set; }
        public List<ControllerInfo> Controllers { get; set; }
    }

    public class ControllerInfo
    {
        public ControllerInfo() 
        {
            Actions = new List<string>();
        }

        public string Name { get; set; }
        public List<string> Actions { get; set; }
    }
}
