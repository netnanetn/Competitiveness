using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Infrastructure
{
    public class FalconLicense
    {
        public string[] Domain { get; set; }
        public string[] IP { get; set; }
        public DateTime ExpiryDate { get; set; }

        public FalconLicense()
        {
            Domain = new string[] { };
            IP = new string[] { };
            ExpiryDate = DateTime.MaxValue;
        }
    }
}
