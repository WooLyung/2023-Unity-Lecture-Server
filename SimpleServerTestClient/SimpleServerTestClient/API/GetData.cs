using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerAPI.API
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

        public int[] GetIntArray()
        {
            int len = GetInt();
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
                result[i] = GetInt();
            return result;
        }

        public float[] GetFloatArray()
        {
            int len = GetInt();
            float[] result = new float[len];
            for (int i = 0; i < len; i++)
                result[i] = GetFloat();
            return result;
        }

        public string[] GetStringArray()
        {
            int len = GetInt();
            string[] result = new string[len];
            for (int i = 0; i < len; i++)
                result[i] = GetString();
            return result;
        }
    }
}
