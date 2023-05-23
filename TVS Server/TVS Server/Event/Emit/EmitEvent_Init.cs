using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TVS_Server.Util;

namespace TVS_Server.Event.Emit
{
    public class EmitEvent_Init : EmitEvent
    {
        public class Map : Decodable
        {
            public int type;
            public int x;
            public int y;

            public byte[] ToBinary()
            {
                return ByteUtil.Unzip(new List<byte[]>() {
                    ByteUtil.From(type),
                    ByteUtil.From(x),
                    ByteUtil.From(y)
                });
            }
        }

        public class Other : Decodable
        {
            public int id;
            public string nickname;
            public string color;
            public float x;
            public float y;
            public float angle;
            public int hp;

            public byte[] ToBinary()
            {
                return ByteUtil.Unzip(new List<byte[]>() {
                    ByteUtil.From(id),
                    ByteUtil.From(nickname),
                    ByteUtil.From(color),
                    ByteUtil.From(x),
                    ByteUtil.From(y),
                    ByteUtil.From(angle),
                    ByteUtil.From(hp)
                });
            }
        }

        public int id;
        public string nickname;
        public string color;
        public float x;
        public float y;
        public int hp;
        public List<Map> map;
        public List<Other> others;

        public override byte[] ToBinary()
        {
            return ByteUtil.UnzipWrapped(0, new List<byte[]>() {
                ByteUtil.From(id),
                ByteUtil.From(nickname),
                ByteUtil.From(color),
                ByteUtil.From(x),
                ByteUtil.From(y),
                ByteUtil.From(hp),
                ByteUtil.ListToBytes(map),
                ByteUtil.ListToBytes(others)
            });
        }
    }
}
