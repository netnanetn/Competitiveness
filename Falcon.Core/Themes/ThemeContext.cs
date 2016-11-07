/// <copyright file="ThemeContext.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System.Linq;
using Falcon;
using Falcon.Infrastructure;

namespace Falcon.Themes
{
    /// <summary>
    /// Theme context
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        private readonly IWorkContext _workContext;
        private readonly IThemeProvider _themeProvider;
        private readonly IThemeService _themeService;

        private bool _themeIsCached;
        private string _cachedThemeName;

        private ThemeType _currentThemeType = ThemeType.Portal;
        private ThemeConfiguration _currentThemeConfiguration;

        private string _currentLayout = "Default";

        public ThemeContext(IWorkContext workContext, IThemeService themeService, IThemeProvider themeProvider)
        {
            _workContext = workContext;
            _themeService = themeService;
            _themeProvider = themeProvider;
        }
        
        /// <summary>
        /// Get or set current theme (e.g. darkOrange)
        /// </summary>
        public string CurrentTheme
        {
            get
            {
                if (_themeIsCached)
                    return _cachedThemeName;

                string theme;
                theme = _themeService.GetTheme(CurrentThemeType);
                
                //ensure that theme exists
                if (!_themeProvider.ThemeConfigurationExists(theme))
                    theme = "Default";

                if (_currentThemeConfiguration == null)
                {
                    _currentThemeConfiguration = _themeProvider.GetThemeConfiguration(theme);
                }
                
                //cache theme
                _cachedThemeName = theme;
                _themeIsCached = true;
                return theme;
            }
            set
            {                
                //clear cache
                _themeIsCached = false;
            }
        }

        public string CurrentLayout
        {
            get
            {
                return _currentLayout;
            }
            set
            {
                _currentLayout = value;
            }
        }

        public string CurrentThemePath
        {
            get
            {
                return EngineContext.Current.FalconConfig.ThemeBasePath + CurrentTheme + "/";
            }
        }

        public ThemeType CurrentThemeType
        {
            get
            {
                return _currentThemeType;
            }
            set
            {
                _currentThemeType = value;
            }
        }

        public ThemeConfiguration CurrentThemeConfiguration
        {
            get
            {
                var theme = CurrentTheme;
                return _currentThemeConfiguration;                
            }
        }
    }
}
