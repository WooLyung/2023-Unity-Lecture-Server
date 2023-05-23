using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS_Server.Util;

namespace TVS_Server.Event.On
{
    public class OnEvent_Update : OnEvent
    {
        public float x;
        public float y;
        public float angle;

        public OnEvent_Update(byte[] buffer)
        {
            x = ByteUtil.ToFloat(buffer, 0);
            y = ByteUtil.ToFloat(buffer, 4);
            angle = ByteUtil.ToFloat(buffer, 8);
        }
    }
}
