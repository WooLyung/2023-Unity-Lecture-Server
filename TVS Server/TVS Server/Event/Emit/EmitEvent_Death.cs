using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS_Server.Event.Inter;
using TVS_Server.Util;

namespace TVS_Server.Event.Emit
{
    public class EmitEvent_Death : EmitEvent
    {
        public int id;
        public int victim;

        public override byte[] ToBinary()
        {
            return ByteUtil.UnzipWrapped(5, new List<byte[]>() {
                ByteUtil.From(id),
                ByteUtil.From(victim),
            });
        }

        public EmitEvent_Death(InterEvent_Death evt)
        {
            id = evt.id;
            victim = evt.victim;
        }
    }
}
