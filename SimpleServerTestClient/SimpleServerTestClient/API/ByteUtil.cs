using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerTestClient.API
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

        public static byte[] From(int[] data)
        {
            int offset = 4;
            int size = 4;
            List<byte[]> array = new List<byte[]>();

            foreach (int entry in data)
            {
                byte[] entryByte = From(entry);
                array.Add(entryByte);
                size += entryByte.Length;
            }

            byte[] result = new byte[size];
            Array.Copy(From(data.Length), 0, result, 0, 4);
            foreach (byte[] entry in array)
            {
                Array.Copy(entry, 0, result, offset, entry.Length);
                offset += entry.Length;
            }

            return result;
        }

        public static byte[] From(float[] data)
        {
            int offset = 4;
            int size = 4;
            List<byte[]> array = new List<byte[]>();

            foreach (float entry in data)
            {
                byte[] entryByte = From(entry);
                array.Add(entryByte);
                size += entryByte.Length;
            }

            byte[] result = new byte[size];
            Array.Copy(From(data.Length), 0, result, 0, 4);
            foreach (byte[] entry in array)
            {
                Array.Copy(entry, 0, result, offset, entry.Length);
                offset += entry.Length;
            }

            return result;
        }

        public static byte[] From(string[] data)
        {
            int offset = 4;
            int size = 4;
            List<byte[]> array = new List<byte[]>();

            foreach (string entry in data)
            {
                byte[] entryByte = From(entry);
                array.Add(entryByte);
                size += entryByte.Length;
            }

            byte[] result = new byte[size];
            Array.Copy(From(data.Length), 0, result, 0, 4);
            foreach (byte[] entry in array)
            {
                Array.Copy(entry, 0, result, offset, entry.Length);
                offset += entry.Length;
            }

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
