using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Falcon.UI.Html
{
    public class SubMenuItem
    {
        /// <summary>
        /// Tên module, action hoặc tham số dùng để xác định liên kết này đang 
        /// ở trạng thái active
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tên hiển thị của liên kết
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Đường liên kết
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Icon đại diện cho liên kết
        /// </summary>
        public string Icon { get; set; }
    }
}
