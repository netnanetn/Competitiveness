using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Falcon.Security
{
    public interface IEncryptionService
    {
        byte[] Encode(byte[] data);
        byte[] Decode(byte[] encodedData);
    }
}
