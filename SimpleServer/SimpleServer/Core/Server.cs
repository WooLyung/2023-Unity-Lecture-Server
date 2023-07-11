using SimpleServer.Data;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace SimpleServer.Core
{
    public class Server
    {
        public ConcurrentDictionary<int, Client> Clients { private set; get; } = new ConcurrentDictionary<int, Client>();
        public ConcurrentQueue<EventData> DataQueue { private set; get; } = new ConcurrentQueue<EventData>();
        private Socket? socket = null;
        private int id = 0;

        public ConcurrentDictionary<String, byte[]> ServerData
        {
            private set;
            get;
        } = new ConcurrentDictionary<string, byte[]>();

        public static void Log(string head, string log)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd/hh:mm:ss") + "] [" + head + "] " + log);
        }

        public void Disconnect(int id)
        {
            Clients.TryRemove(id, out _);
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
                    EventData data;
                    while (DataQueue.TryDequeue(out data))
                    {
                        foreach (Client client in Clients.Values)
                            if (client.Group == data.Group)
                                client.EmitEvent(data.ByteData);
                    }
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