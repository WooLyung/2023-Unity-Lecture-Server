using SimpleServer.Data;
using SimpleServer.Util;
using System;
using System.Net.Sockets;

namespace SimpleServer.Core
{
    public class Client
    {
        private Server server;
        private Socket socket;
        private Thread thread;
        private int id;
        private bool isRunning = true;

        public String Group
        {
            private set;
            get;
        } = "Null";

        public Client(Socket socket, Server server, int id)
        {
            thread = new Thread(ThreadFunction);
            this.socket = socket;
            this.server = server;
            this.id = id;
        }

        public void Start() => thread.Start();

        public void EmitEvent(byte[] data)
        {
            // todo : size
            socket.Send(data);
        }

        public void ThreadFunction()
        {
            Server.Log("INFO", $"client #{id} connected");

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

                    if (code == 0) // regist
                    {
                        // [len][group-name]
                        int len = ByteUtil.ToInt(dataBuffer, 0);
                        string group = ByteUtil.ToString(dataBuffer, 4, len);
                        Group = group;
                        Server.Log("INFO", $"client #{id} was registed in group named {group}.");
                    }
                    else if (code == 1) // get server data
                    {
                        // [len][key]
                        int len = ByteUtil.ToInt(dataBuffer, 0);
                        string key = ByteUtil.ToString(dataBuffer, 4, len);
                        byte[] data;
                        if (server.ServerData.TryGetValue(key, out data))
                            EmitEvent(data);
                        else
                            EmitEvent(null);
                    }
                    else if (code == 2) // set server data
                    {
                        // [len][key][data]
                        int len = ByteUtil.ToInt(dataBuffer, 0);
                        string key = ByteUtil.ToString(dataBuffer, 4, len);
                        byte[] data = new byte[size - 4 - len];
                        Array.Copy(dataBuffer, 4 + len, data, 0, size - 4 - len);

                        server.ServerData.TryAdd(key, data);
                    }
                    else if (code == 3) // post client data
                    {
                        // [len][group][data]
                        int len = ByteUtil.ToInt(dataBuffer, 0);
                        string group = ByteUtil.ToString(dataBuffer, 4, len);
                        byte[] data = new byte[size - 4 - len];
                        Array.Copy(dataBuffer, 4 + len, data, 0, size - 4 - len);

                        server.DataQueue.Enqueue(new EventData(group, data));
                    }
                }
                catch (SocketException e)
                {
                    Disconnect();
                    break;
                }
                catch (Exception e)
                {
                    Server.Log("ERROR", e.Message);
                }
            }
        }

        private void Disconnect()
        {
            isRunning = false;
            if (!socket.Connected)
                socket.Disconnect(true);
            server.Disconnect(id);
        }
    }
}
