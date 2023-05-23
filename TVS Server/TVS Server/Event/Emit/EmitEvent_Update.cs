using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS_Server.Util;
using static TVS_Server.Event.Emit.EmitEvent_Init;

namespace TVS_Server.Event.Emit
{
    public class EmitEvent_Update : EmitEvent
    {
        public class Player : Decodable
        {
            public int id;
            public float x;
            public float y;
            public float angle;

            public byte[] ToBinary()
            {
                return ByteUtil.Unzip(new List<byte[]>() {
                    ByteUtil.From(id),
                    ByteUtil.From(x),
                    ByteUtil.From(y),
                    ByteUtil.From(angle)
                });
            }
        }

        public List<Player> players;

        public override byte[] ToBinary()
        {
            return ByteUtil.UnzipWrapped(1, new List<byte[]>() {
                ByteUtil.ListToBytes(players)
            });
        }
    }
}
