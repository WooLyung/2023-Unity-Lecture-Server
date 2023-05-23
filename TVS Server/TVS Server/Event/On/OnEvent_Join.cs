using System.Runtime.Serialization;
using TVS_Server.Event.Emit;
using TVS_Server.Util;

namespace TVS_Server.Event.On
{
    public class OnEvent_Join : OnEvent
    {
        public string nickname;
        public string color;

        public OnEvent_Join(byte[] buffer)
        {
            nickname = ByteUtil.ToString(buffer, 0, 16);
            color = ByteUtil.ToString(buffer, 16, 6);
        }
    }
}