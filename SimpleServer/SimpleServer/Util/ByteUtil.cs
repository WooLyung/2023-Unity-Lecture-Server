using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer.Util
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
            return System.Text.Encoding.UTF8.GetBytes(data.ToCharArray());
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
    }
}
