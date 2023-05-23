using System;
using System.Net.Sockets;
using TVS_Server.Event.Emit;
using TVS_Server.Event.On;
using TVS_Server.Player;

namespace TVS_Server.Core
{
    public class Client
    {
        private Server server;
        private Socket socket;
        private Thread thread;
        private int id;
        public PlayerData playerData { get; private set; }
        
        public Client(Socket socket, Server server, int id)
        {
            thread = new Thread(ThreadFunction);
            this.socket = socket;
            this.server = server;
            this.id = id;
            playerData = new PlayerData();
        }

        public void Start() => thread.Start();

        public void EmitEvent(byte[] data) => socket.Send(data);

        public void ThreadFunction()
        {
            Server.Log("INFO", "client connected");

            while (true)
            {
                // 타임 아웃에서 종료 처리

                try
                {
                    byte[] intBuffer = new byte[4];
                    int byteReceived;

                    byteReceived = socket.Receive(intBuffer);
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

                    if (code == 0) // init
                    {
                        OnEvent_Join evt = new OnEvent_Join(dataBuffer);

                        playerData.x = 0.0f;
                        playerData.y = 0.0f;
                        playerData.angle = 0.0f;
                        playerData.hp = 100;
                        playerData.nickname = evt.nickname;
                        playerData.color = evt.color;
                        playerData.status = 1;

                        // 모두에게 join을 알림

                        EmitEvent_Init emit = new EmitEvent_Init();
                        emit.x = playerData.x;
                        emit.y = playerData.y;
                        emit.hp = playerData.hp;
                        emit.nickname = playerData.nickname;
                        emit.color = playerData.color;
                        emit.id = id;
                        
                        emit.map = new List<EmitEvent_Init.Map>();
                        
                        emit.others = new List<EmitEvent_Init.Other>();
                        foreach (var pair in server.Clients)
                        {
                            Client cl = pair.Value;
                            EmitEvent_Init.Other other = new EmitEvent_Init.Other();
                            other.x = cl.playerData.x;
                            other.y = cl.playerData.y;
                            other.angle = cl.playerData.angle;
                            other.hp = cl.playerData.hp;
                            other.nickname = cl.playerData.nickname;
                            other.color = cl.playerData.color;
                            emit.others.Add(other);
                        }

                        EmitEvent(emit.ToBinary());
                    }
                    else if (code == 1) // update
                    {
                        OnEvent_Update evt = new OnEvent_Update(dataBuffer);
                        playerData.x = evt.x;
                        playerData.y = evt.y;
                        playerData.angle = evt.angle;
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
