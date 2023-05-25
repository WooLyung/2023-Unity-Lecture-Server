using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using TVS_Server.Event.Emit;
using TVS_Server.Player;

namespace TVS_Server.Core
{
    public class Server
    {
        public ConcurrentDictionary<int, Client> Clients { private set; get; } = new ConcurrentDictionary<int, Client>();
        public ConcurrentQueue<EmitEvent> EventQueue { private set; get; } = new ConcurrentQueue<EmitEvent>();
        private Socket? socket = null;
        private int id = 0;

        public static void Log(string head, string log)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd/hh:mm:ss") + "] [" + head + "] " + log);
        }

        public void Disconnect(int id)
        {
            Client client;
            Clients.TryRemove(id, out client);
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
    }
}