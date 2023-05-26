using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS_Server.Event.Inter;
using TVS_Server.Util;

namespace TVS_Server.Event.Emit
{
    public class EmitEvent_Leave : EmitEvent
    {
        public int id;

        public override byte[] ToBinary()
        {
            return ByteUtil.UnzipWrapped(3, new List<byte[]>() {
                ByteUtil.From(id)
            });
        }

        public EmitEvent_Leave(InterEvent_Leave evt)
        {
            id = evt.id;
        }
    }
}
