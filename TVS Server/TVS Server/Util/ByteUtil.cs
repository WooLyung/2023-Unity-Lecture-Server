using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVS_Server.Util
{
    public static class ByteUtil
    {
        public static byte[] Unzip(List<byte[]> bytes)
        {
            int size = 0, idx = 0;
            foreach (byte[] bArray in bytes)
                size += bArray.Length;

            byte[] result = new byte[size];
            foreach (byte[] bArray in bytes)
            {
                bArray.CopyTo(result, idx);
                idx += bArray.Length;
            }

            return result;
        }

        public static byte[] ListToBytes<T>(List<T> data)
        {
            List<byte[]> bytes = new List<byte[]> {
                BitConverter.GetBytes(data.Count)
            };

            foreach (Decodable d in data)
                bytes.Add(d.ToBinary());

            return Unzip(bytes);
        }

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

        public static byte[] UnzipWrapped(int code, List<byte[]> bytes)
        {
            int size = 0, idx = 8;
            foreach (byte[] bArray in bytes)
                size += bArray.Length;

            byte[] result = new byte[size + 8];
            From(size).CopyTo(result, 0);
            From(code).CopyTo(result, 4);
            foreach (byte[] bArray in bytes)
            {
                bArray.CopyTo(result, idx);
                idx += bArray.Length;
            }

            return result;
        }
    }
}
