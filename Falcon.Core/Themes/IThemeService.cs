/// <copyright file="IThemeService.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description>Interface thao tác với csdl lưu cấu hình giao diện.</description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon.Themes
{
    public interface IThemeService
    {
        /// <summary>
        /// Lấy giao diện hiện tại của các trang Portal
        /// </summary>
        /// <param name="themeType"></param>
        /// <returns></returns>
        string GetTheme(ThemeType themeType = ThemeType.Portal);

        /// <summary>
        /// Thiết lập giao diện cho các trang Portal
        /// </summary>
        /// <param name="themeName"></param>
        /// <param name="themeType"></param>
        void SetTheme(string themeName, ThemeType themeType = ThemeType.Portal);

        IEnumerable<Theme> GetAllThemes();
    }
}
