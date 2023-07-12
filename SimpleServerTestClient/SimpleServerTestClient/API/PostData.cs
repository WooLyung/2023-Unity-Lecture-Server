using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerTestClient.API
{
    public class PostData
    {
        private List<byte[]> bytes = new List<byte[]>();

        public PostData AddInt(int value)
        {
            bytes.Add(ByteUtil.From(value));
            return this;
        }

        public PostData AddFloat(float value)
        {
            bytes.Add(ByteUtil.From(value));
            return this;
        }

        public PostData AddString(string value)
        {
            bytes.Add(ByteUtil.From(value));
            return this;
        }

        public byte[] ToBinary()
        {
            int size = 0;
            int offset = 0;
            foreach (byte[] b in bytes)
                size += b.Length;

            byte[] result = new byte[size];
            foreach (byte[] b in bytes)
            {
                Array.Copy(b, 0, result, offset, b.Length);
                offset += b.Length;
            }

            return result;
        }
    }
}
