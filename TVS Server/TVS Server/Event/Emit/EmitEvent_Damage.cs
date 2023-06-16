using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS_Server.Event.Inter;
using TVS_Server.Util;

namespace TVS_Server.Event.Emit
{
    public class EmitEvent_Damage : EmitEvent
    {
        public int id;
        public int victim;
        public int damage;

        public override byte[] ToBinary()
        {
            return ByteUtil.UnzipWrapped(4, new List<byte[]>() {
                ByteUtil.From(id),
                ByteUtil.From(victim),
                ByteUtil.From(damage)
            });
        }

        public EmitEvent_Damage(InterEvent_Damage evt)
        {
            id = evt.id;
            victim = evt.victim;
            damage = evt.damage;
        }
    }
}
