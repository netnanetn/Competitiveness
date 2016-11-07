/// <copyright file="PermissionInfo.cs" company="DKT">
/// Copyright (c) 2011 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description>Chứa thông tin phân quyền</description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Falcon.Security
{
    public class PermissionInfo
    {
        public string Module;
        public string Controller;
        public List<string> Actions;
    }
}
