using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using TVS_Server.Event.Emit;

namespace TVS_Server
{
    public class Server
    {
        private ConcurrentDictionary<int, Client> clients = new ConcurrentDictionary<int, Client>();
        private ConcurrentQueue<EmitEvent> eventQueue = new ConcurrentQueue<EmitEvent>();
        private Socket? socket = null;
        private int id = 0;

        public static void Log(string head, string log)
        {
            Console.WriteLine(head + " " + log);
        }

        public void Start(int port)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(endPoint);
                socket.Listen(10);

                Log("INFO", "Server Start");

                Thread eventThread = new Thread(EventThread);
                eventThread.Start();

                while (true)
                {
                    Socket clientSocket = socket.Accept();
                    Client client = new Client(clientSocket, this, id);
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
            Log("INFO", "Event Thread Start");

            while (true)
            {

            }
        }

        public void Join(int id, Client client)
        {
            clients.TryAdd(id, client);
        }

        public void Disconnect(int id)
        {
            Client? client = null;
            clients.TryRemove(id, out client);
            client?.Disconnect();
        }
    }
}