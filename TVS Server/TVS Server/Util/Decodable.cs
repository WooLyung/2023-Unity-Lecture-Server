using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVS_Server.Util
{
    public interface Decodable
    {
        public byte[] ToBinary();
    }
}