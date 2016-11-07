using System.Collections.Generic;

namespace Falcon.Themes
{
    public interface IThemeProvider
    {
        /// <summary>
        /// Lấy thông tin về giao diện hệ thống & giao diện mẫu gian hàng
        /// </summary>
        /// <param name="themeName"></param>
        /// <param name="isStoreTheme"></param>
        /// <returns></returns>
        ThemeConfiguration GetThemeConfiguration(string themeName);

        IList<ThemeConfiguration> GetThemeConfigurations();

        bool ThemeConfigurationExists(string themeName);

        void ReloadConfigurations();
    }
}
