using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerAPI.API
{
    public static class ByteUtil
    {
        public static byte[] From(int data)
        {
            return BitConverter.GetBytes(data);
        }

        public static byte[] From(float data)
        {
            return BitConverter.GetBytes(data);
        }

        public static byte[] From(string data)
        {
            byte[] strData = Encoding.UTF8.GetBytes(data.ToCharArray());
            byte[] result = new byte[4 + strData.Length];
            Array.Copy(From(data.Length), 0, result, 0, 4);
            Array.Copy(strData, 0, result, 4, strData.Length);

            return result;
        }

        public static int ToInt(byte[] data, int offset)
        {
            return BitConverter.ToInt32(data, offset);
        }

        public static float ToFloat(byte[] data, int offset)
        {
            return BitConverter.ToSingle(data, offset);
        }

        public static string ToString(byte[] data, int offset, int len)
        {
            return Encoding.UTF8.GetString(data, offset, len);
        }

        public static string ToString(byte[] data, int offset)
        {
            int len = ToInt(data, offset);
            return ToString(data, offset + 4, len);
        }
    }
}
