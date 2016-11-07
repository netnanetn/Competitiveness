using System;
using System.Configuration;
using System.Xml;

namespace Falcon.Configuration
{
    /// <summary>
    /// Represents a FalconConfig
    /// </summary>
    public partial class FalconConfig : IConfigurationSectionHandler
    {
        
        #region Methods

        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="section">Section XML node.</param>
        /// <returns>The created section handler object.</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            FalconConfig config = new FalconConfig();

            var registrationKeyNode = section.SelectSingleNode("RegistrationKey");
            if (registrationKeyNode == null)
            {
                throw new Exception("RegistrationKey must be existed on FalconConfig section");
            }
            config.RegistrationKey = registrationKeyNode.InnerText;

            var domainNameNode = section.SelectSingleNode("DomainName");
            if (domainNameNode != null)
            {
                config.DomainName = domainNameNode.InnerText;
            }

            var mediaDomainNameNode = section.SelectSingleNode("MediaDomainName");
            if (mediaDomainNameNode != null)
            {
                config.MediaDomainName = mediaDomainNameNode.InnerText;
            }

            var staticDomainNameNode = section.SelectSingleNode("StaticDomainName");
            if (staticDomainNameNode != null)
            {
                config.StaticDomainName = staticDomainNameNode.InnerText;
            }

            var loggingDomainNameNode = section.SelectSingleNode("LoggingDomainName");
            if (loggingDomainNameNode != null)
            {
                config.LoggingDomainName = loggingDomainNameNode.InnerText;
            }

            var dynamicDiscoveryNode = section.SelectSingleNode("DynamicDiscovery");
            if (dynamicDiscoveryNode != null)
            {
                var attribute = dynamicDiscoveryNode.Attributes["Enabled"];
                if (attribute != null)
                    config.DynamicDiscovery = Convert.ToBoolean(attribute.Value);
            }

            var themeNode = section.SelectSingleNode("Themes");
            if (themeNode != null)
            {               
                var attribute = themeNode.Attributes["basePath"];
                if (attribute != null)
                    config.ThemeBasePath = attribute.Value;

                attribute = themeNode.Attributes["storeBasePath"];
                if (attribute != null)
                    config.ThemeStoreBasePath = attribute.Value;
            }

            //config.ScheduleTasks = section.SelectSingleNode("ScheduleTasks");

            return config;
        }
        
            #endregion

        #region Properties

        public string RegistrationKey { get; set; }

        public string DomainName { get; set; }

        /// <summary>
        /// Domain phục vụ ảnh, thumbnail, flash, video...
        /// </summary>
        public string MediaDomainName { get; set; }

        /// <summary>
        /// Domain phục vụ file tĩnh: js, css, favicon
        /// </summary>
        public string StaticDomainName { get; set; }

        public string LoggingDomainName { get; set; }

        /// <summary>
        /// In addition to configured assemblies examine and load assemblies in the bin directory.
        /// </summary>
        public bool DynamicDiscovery { get; set; }   
        
        /// <summary>
        /// Gets or sets a schedule tasks section
        /// </summary>
        //public XmlNode ScheduleTasks { get; set; }

        /// <summary>
        /// Đường dẫn giao diện hệ thống (sản phẩm, rao vặt)
        /// </summary>
        public string ThemeBasePath { get; set; }

        /// <summary>
        /// Đường dẫn giao diện mẫu của gian hàng
        /// </summary>
        public string ThemeStoreBasePath { get; set; }
        #endregion
        
    }
}
