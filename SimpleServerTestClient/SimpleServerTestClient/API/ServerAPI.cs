using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace SimpleServerTestClient.API
{
    class ServerAPI
    {
        private static ServerAPI instance = null;
        
        private static ServerAPI Instance
        {
            get
            {
                if (instance == null)
                    return instance = new ServerAPI();
                return instance;
            }
        }

        // internal

        // private IPAddress ip = IPAddress.Parse("34.64.40.5");
        private IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port = 8462;
        private Socket socket = null;
        public bool isRunning { private set; get; } = true;

        private byte[] preServerData = null;

        private void Disconnect()
        {
            isRunning = false;
            if (!socket.Connected)
                socket.Disconnect(true);
        }

        private bool Connect()
        {
            if (socket == null)
            {
                try
                {
                    IPEndPoint remoteEP = new IPEndPoint(ip, port);
                    socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                    socket.ReceiveBufferSize = 8192;
                    socket.SendTimeout = 1000;
                    socket.Connect(remoteEP);
                }
                catch (Exception e)
                {
                    socket = null;
                    return false;
                }
            }
            return true;
        }

        private void ReadThread()
        {
            while (isRunning)
            {
                try
                {
                    if (!socket.Connected)
                    {
                        Disconnect();
                        break;
                    }

                    byte[] intBuffer = new byte[4];
                    int byteReceived;

                    byteReceived = socket.Receive(intBuffer);
                    if (byteReceived == 0)
                    {
                        Disconnect();
                        break;
                    }

                    int size = BitConverter.ToInt32(intBuffer);
                    byteReceived = socket.Receive(intBuffer);
                    int code = BitConverter.ToInt32(intBuffer);

                    byte[] buffer = new byte[1024];
                    byte[] dataBuffer = new byte[size];
                    int sumByte = 0;
                    
                    while (sumByte < size)
                    {
                        byteReceived = socket.Receive(buffer, Math.Min(1024, size - sumByte), SocketFlags.None);
                        Array.Copy(buffer, 0, dataBuffer, sumByte, byteReceived);
                        sumByte += byteReceived;
                    }

                    if (code == 0) // server data
                    {
                        preServerData = dataBuffer;
                    }
                    else if (code == 1) // client data
                    {

                    }
                }
                catch (Exception e)
                {
                    // Debug.Log(e);
                }
            }
        }

        private void Send(byte[] data)
        {
            if (socket.Connected)
                socket.Send(data);
        }

        private void Regist(string group)
        {
            byte[] b0 = ByteUtil.From(0);
            byte[] b1 = ByteUtil.From(group);
            byte[] bs = ByteUtil.From(b1.Length);
            byte[] result = new byte[b0.Length + b1.Length + bs.Length];

            Array.Copy(bs, 0, result, 0, bs.Length);
            Array.Copy(b0, 0, result, bs.Length, b0.Length);
            Array.Copy(b1, 0, result, bs.Length + b0.Length, b1.Length);

            Send(result);
        }

        // API

        private void _Start(string group)
        {
            while (!Connect());
            Regist(group);
            Thread thread = new Thread(ReadThread);
            thread.Start();
        }

        public static void Start(string group)
        {
            Instance._Start(group);
        }

        private void _SetServerData(string key, byte[] data)
        {
            byte[] b0 = ByteUtil.From(2);
            byte[] b1 = ByteUtil.From(key);
            byte[] bs = ByteUtil.From(b1.Length + data.Length);
            byte[] result = new byte[b0.Length + b1.Length + data.Length + bs.Length];

            Array.Copy(bs, 0, result, 0, bs.Length);
            Array.Copy(b0, 0, result, bs.Length, b0.Length);
            Array.Copy(b1, 0, result, bs.Length + b0.Length, b1.Length);
            Array.Copy(data, 0, result, bs.Length + b0.Length + b1.Length, data.Length);

            Send(result);
        }

        private void _SetServerData(string key, int data)
        {
            _SetServerData(key, ByteUtil.From(data));
        }

        private void _SetServerData(string key, float data)
        {
            _SetServerData(key, ByteUtil.From(data));
        }

        private void _SetServerData(string key, string data)
        {
            _SetServerData(key, ByteUtil.From(data));
        }

        private void _SetServerData(string key, int[] data)
        {
            _SetServerData(key, ByteUtil.From(data));
        }

        private void _SetServerData(string key, float[] data)
        {
            _SetServerData(key, ByteUtil.From(data));
        }

        private void _SetServerData(string key, string[] data)
        {
            _SetServerData(key, ByteUtil.From(data));
        }

        public static void SetServerData(string key, int data)
        {
            Instance._SetServerData(key, data);
        }

        public static void SetServerData(string key, float data)
        {
            Instance._SetServerData(key, data);
        }

        public static void SetServerData(string key, string data)
        {
            Instance._SetServerData(key, data);
        }

        public static void SetServerData(string key, int[] data)
        {
            Instance._SetServerData(key, data);
        }

        public static void SetServerData(string key, float[] data)
        {
            Instance._SetServerData(key, data);
        }

        public static void SetServerData(string key, string[] data)
        {
            Instance._SetServerData(key, data);
        }

        private byte[] RequestServerData(string key)
        {
            byte[] b0 = ByteUtil.From(1);
            byte[] b1 = ByteUtil.From(key);
            byte[] bs = ByteUtil.From(b1.Length);
            byte[] result = new byte[b0.Length + b1.Length + bs.Length];

            Array.Copy(bs, 0, result, 0, bs.Length);
            Array.Copy(b0, 0, result, bs.Length, b0.Length);
            Array.Copy(b1, 0, result, bs.Length + b0.Length, b1.Length);

            Send(result);

            while (preServerData == null) ;
            byte[] o = preServerData;
            preServerData = null;
            return o;
        }

        private int GetServerDataInt_(string key, int dvalue)
        {
            byte[] data = RequestServerData(key);

            if (data.Length == 0)
                return dvalue;
            return ByteUtil.ToInt(data, 0);
        }

        private float GetServerDataFloat_(string key, float dvalue)
        {
            byte[] data = RequestServerData(key);

            if (data.Length == 0)
                return dvalue;
            return ByteUtil.ToFloat(data, 0);
        }

        private string GetServerDataString_(string key, string dvalue)
        {
            byte[] data = RequestServerData(key);

            if (data.Length == 0)
                return dvalue;
            return ByteUtil.ToString(data, 0);
        }

        public static int GetServerDataInt(string key, int dvalue)
        {
            return Instance.GetServerDataInt_(key, dvalue);
        }

        public static float GetServerDataFloat(string key, float dvalue)
        {
            return Instance.GetServerDataFloat_(key, dvalue);
        }

        public static string GetServerDataString(string key, string dvalue)
        {
            return Instance.GetServerDataString_(key, dvalue);
        }
    }
}
