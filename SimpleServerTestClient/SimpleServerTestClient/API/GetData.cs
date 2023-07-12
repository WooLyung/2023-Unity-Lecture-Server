using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerTestClient.API
{
    public class GetData
    {
        private byte[] data;
        private int offset = 0;

        public GetData(byte[] data)
        {
            this.data = data;
        }

        public int GetInt()
        {
            int value = ByteUtil.ToInt(data, offset);
            offset += 4;
            return value;
        }

        public float GetFloat()
        {
            float value = ByteUtil.ToFloat(data, offset);
            offset += 4;
            return value;
        }

        public string GetString()
        {
            string value = ByteUtil.ToString(data, offset);
            offset += 4 + value.Length;
            return value;
        }
    }
}
