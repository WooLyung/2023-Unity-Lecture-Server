using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using TVS_Server.Event.Emit;
using TVS_Server.Event.Inter;
using TVS_Server.Player;

namespace TVS_Server.Core
{
    public class Server
    {
        public ConcurrentDictionary<int, Client> Clients { private set; get; } = new ConcurrentDictionary<int, Client>();
        public ConcurrentQueue<InterEvent> EventQueue { private set; get; } = new ConcurrentQueue<InterEvent>();
        private Socket? socket = null;
        private int id = 0;

        public static void Log(string head, string log)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd/hh:mm:ss") + "] [" + head + "] " + log);
        }

        public void Disconnect(int id)
        {
            Clients.TryRemove(id, out _);

            InterEvent_Leave inter = new InterEvent_Leave();
            inter.id = id;
            EventQueue.Enqueue(inter);
        }

        public void Start(int port)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                socket.ReceiveBufferSize = 8192;
                socket.Bind(endPoint);
                socket.Listen(10);

                Log("INFO", "server started");

                Thread eventThread = new Thread(EventThread);
                eventThread.Start();

                while (true)
                {
                    Socket clientSocket = socket.Accept();
                    clientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                    clientSocket.ReceiveBufferSize = 8192;
                    clientSocket.ReceiveTimeout = 1000;
                    clientSocket.SendTimeout = 1000;
                    Client client = new Client(clientSocket, this, id);
                    Clients.TryAdd(id, client);
                    client.Start();
                    id++;
                }
            }
            catch (Exception e)
            {
                Log("ERROR", e.Message);
            }
            finally
            {
                socket?.Disconnect(true);
            }
        }

        private void EventThread()
        {
            Log("INFO", "event thread started");

            while (true)
            {
                try
                {
                    // event queue
                    InterEvent evt;
                    while (EventQueue.TryDequeue(out evt))
                    {
                        switch (evt)
                        {
                            case InterEvent_Join x:
                                OnJoin(x);
                                break;
                            case InterEvent_Leave x:
                                OnLeave(x);
                                break;
                        }
                    }

                    // update event
                    EmitEvent_Update emit = new EmitEvent_Update();
                    emit.players = new List<EmitEvent_Update.Player>();

                    foreach (var pair in Clients)
                    {
                        PlayerData playerData = pair.Value.playerData;
                        EmitEvent_Update.Player player = new EmitEvent_Update.Player();
                        player.x = playerData.x;
                        player.y = playerData.y;
                        player.angle = playerData.angle;
                        player.id = pair.Key;
                        player.hp = playerData.hp;
                        emit.players.Add(player);
                    }

                    byte[] binary = emit.ToBinary();
                    foreach (var pair in Clients)
                        pair.Value.EmitEvent(binary);
                }
                catch (Exception e)
                {
                    Log("ERROR", e.Message);
                }

                Thread.Sleep(20);
            }
        }

        private void OnJoin(InterEvent_Join evt)
        {
            EmitEvent_Join emit = new EmitEvent_Join(evt);
            byte[] binary = emit.ToBinary();
            foreach (var pair in Clients)
                if (pair.Key != evt.id)
                    pair.Value.EmitEvent(binary);

            Server.Log("INFO", $"client #{evt.id} joined");
        }

        private void OnLeave(InterEvent_Leave evt)
        {
            EmitEvent_Leave emit = new EmitEvent_Leave(evt);
            byte[] binary = emit.ToBinary();
            foreach (var pair in Clients)
                if (pair.Key != evt.id)
                    pair.Value.EmitEvent(binary);

            Server.Log("INFO", $"client #{evt.id} disconnected");
        }
    }
}