using System.Runtime.Serialization;
using TVS_Server.Event.Emit;
using TVS_Server.Util;

namespace TVS_Server.Event.On
{
    public class OnEvent_Damage : OnEvent
    {
        public int id;
        public int victim;
        public int damage;

        public OnEvent_Damage(byte[] buffer)
        {
            id = ByteUtil.ToInt(buffer, 0);
            victim = ByteUtil.ToInt(buffer, 4);
            damage = ByteUtil.ToInt(buffer, 8);
        }
    }
}