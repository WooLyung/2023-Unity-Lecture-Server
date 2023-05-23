using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TVS_Server.Event;
using TVS_Server.Event.On;
using TVS_Server.Player;

namespace TVS_Server
{
    public class Client
    {
        private Server server;
        private Socket socket;
        private Thread thread;
        private int id;
        private PlayerData playerData;

        public Client(Socket socket, Server server, int id)
        {
            thread = new Thread(ThreadFunction);
            this.socket = socket;
            this.server = server;
            this.id = id;
            playerData = new PlayerData();
        }

        public void Start() => thread.Start();

        public void Disconnect()
        {
            socket.Disconnect(true);
        }

        public void ThreadFunction()
        {
            Server.Log("INFO", "Client Connected");

            while (true)
            {
                try
                {
                    byte[] intBuffer = new byte[4];
                    int byteReceived;

                    byteReceived = socket.Receive(intBuffer);
                    int size = BitConverter.ToInt32(intBuffer);
                    byteReceived = socket.Receive(intBuffer);
                    int code = BitConverter.ToInt32(intBuffer);

                    byte[] buffer = new byte[1024];
                    byte[] convertBuffer = new byte[size];
                    int sumByte = 0;

                    while (sumByte < size)
                    {
                        byteReceived = socket.Receive(buffer);
                        Array.Copy(buffer, 0, convertBuffer, sumByte, byteReceived);
                        sumByte += byteReceived;
                    }

                    if (code == 0) // init
                    {
                        OnEvent_Join evt = new OnEvent_Join(convertBuffer);
                        // 모두에게 join을 알림
                        playerData.X = 0.0f;
                        playerData.Y = 0.0f;
                        playerData.Angle = 0.0f;
                        playerData.Hp = 100;
                        playerData.Nickname = evt.nickname;
                        playerData.Color = evt.color;
                    }
                }
                catch (Exception e)
                {
                    Server.Log("ERROR", e.Message);
                }
            }
        }
    }
}
