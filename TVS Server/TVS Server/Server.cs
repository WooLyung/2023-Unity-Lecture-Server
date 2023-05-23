using System.Net;
using System.Net.Sockets;

namespace TVS_Server
{
    public class Server
    {
        private TcpListener? listener = null;

        public static void Log(string head, string log)
        {
            Console.WriteLine(head + " " + log);
        }

        public void Start()
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 9172;

                listener = new TcpListener(ipAddress, port);

                listener.Start();
                Log("INFO", "Server Start");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientThread thread = new ClientThread(client);
                    thread.Start();
                }
            }
            catch (Exception e)
            {
                Log("ERROR", e.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
    }
}