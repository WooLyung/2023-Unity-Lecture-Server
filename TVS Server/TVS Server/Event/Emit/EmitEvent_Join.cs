using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS_Server.Event.Inter;
using TVS_Server.Util;

namespace TVS_Server.Event.Emit
{
    public class EmitEvent_Join : EmitEvent
    {
        public int id;
        public string nickname;
        public string color;
        public float x;
        public float y;
        public float angle;
        public int hp;

        public override byte[] ToBinary()
        {
            return ByteUtil.UnzipWrapped(2, new List<byte[]>() {
                ByteUtil.From(id),
                ByteUtil.From(nickname),
                ByteUtil.From(color),
                ByteUtil.From(x),
                ByteUtil.From(y),
                ByteUtil.From(angle),
                ByteUtil.From(hp)
            });
        }

        public EmitEvent_Join(InterEvent_Join evt)
        {
            id = evt.id;
            nickname = evt.nickname;
            color = evt.color;
            x = evt.x;
            y = evt.y;
            angle = evt.angle;
            hp = evt.hp;
        }
    }
}
