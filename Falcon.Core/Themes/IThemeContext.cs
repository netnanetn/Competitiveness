/// <copyright file="IThemeContext.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

namespace Falcon.Themes
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IThemeContext
    {
        ThemeType CurrentThemeType { get; set; }
        /// <summary>
        /// Get or set current graphical theme (e.g. Hangtot)
        /// </summary>
        string CurrentTheme { get; set; }

        string CurrentLayout { get; set; }

        string CurrentThemePath { get; }

        ThemeConfiguration CurrentThemeConfiguration { get; }
    }
}
