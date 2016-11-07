/// <copyright file="ThemeProvider.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Falcon.Configuration;

namespace Falcon.Themes
{
    /// <summary>
    /// Cung cấp các hàm thao tác với kho giao diện của hệ thống
    /// Riêng với giao diện riêng của gian hàng, để giảm thiểu tgian khởi động
    /// sẽ được load từng giao diện ở request đầu tiên
    /// </summary>
    public class ThemeProvider : IThemeProvider
    {
        #region Fields
        /// <summary>
        /// Chứa cấu hình các theme dành cho Portal: sàn giao dịch, rao vặt, hỏi đáp, review...
        /// </summary>
        private IList<ThemeConfiguration> _themeConfigurations = new List<ThemeConfiguration>();

        /// <summary>
        /// Đường dẫn gốc cho các theme của Portal
        /// </summary>
        private string basePath = string.Empty;
        private FalconConfig _falconConfig;
        private IWebHelper _webHelper;

        #endregion

        #region Constructors

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="falconConfig"></param>
        /// <param name="webHelper"></param>
        public ThemeProvider(FalconConfig falconConfig, IWebHelper webHelper)
        {
            _falconConfig = falconConfig;
            _webHelper = webHelper;

            basePath = webHelper.MapPath(falconConfig.ThemeBasePath);

            LoadConfigurations();
        }

        #endregion 
        
        #region IThemeProvider
        /// <summary>
        /// Lấy cấu hình giao diện của Portal
        /// </summary>
        /// <param name="themeName">Tên của giao diện</param>
        /// <returns>ThemeConfiguration</returns>
        public ThemeConfiguration GetThemeConfiguration(string themeName)
        {
            return
            _themeConfigurations.SingleOrDefault(
                x => x.ThemeName.Equals(themeName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IList<ThemeConfiguration> GetThemeConfigurations()
        {
            return _themeConfigurations;
        }

        /// <summary>
        /// Kiểm tra giao diện có tồn tại không
        /// </summary>
        /// <param name="themeName">Tên của giao diện</param>
        /// <returns>True nếu giao diện có tồn tại, False nếu không tồn tại</returns>
        public bool ThemeConfigurationExists(string themeName)
        {
            if (string.IsNullOrEmpty(themeName)) return false;
            return GetThemeConfigurations().Any(configuration => configuration.ThemeName.Equals(themeName, StringComparison.InvariantCultureIgnoreCase));
        }

        public void ReloadConfigurations()
        {
            IList<ThemeConfiguration> tmp = new List<ThemeConfiguration>();

            foreach (string themeName in Directory.GetDirectories(basePath))
            {
                var configuration = CreateThemeConfiguration(themeName, _falconConfig.ThemeBasePath);
                if (configuration != null)
                {
                    tmp.Add(configuration);               
                }
            }

            _themeConfigurations = tmp;
        }

        #endregion

        #region Utility

        private void LoadConfigurations()
        {
            //TODO:Use IFileStorage?
            foreach (string themeName in Directory.GetDirectories(basePath))
            {
                var configuration = CreateThemeConfiguration(themeName, _falconConfig.ThemeBasePath);
                if(configuration != null)
                {
                    _themeConfigurations.Add(configuration);
                }
            }
        }

        private ThemeConfiguration CreateThemeConfiguration(string themePath, string basePath)
        {
            var themeDirectory = new DirectoryInfo(themePath);
            var themeConfigFile = new FileInfo(Path.Combine(themeDirectory.FullName, "theme.config"));

            if(themeConfigFile.Exists)
            {
                var doc = new XmlDocument();
                doc.Load(themeConfigFile.FullName);
                return new ThemeConfiguration(themeDirectory.Name, themeDirectory.FullName, basePath + themeDirectory.Name, doc);
            }

            return null;
        }

        #endregion
    }
}
