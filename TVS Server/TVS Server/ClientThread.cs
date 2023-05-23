using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TVS_Server
{
    public class ClientThread
    {
        TcpClient client;
        Thread thread;

        public ClientThread(TcpClient client)
        {
            thread = new Thread(ThreadFunction);
            this.client = client;
        }

        public void Start() => thread.Start();

        public void ThreadFunction(object obj)
        {
            Console.WriteLine("Client Entered");
        }
    }
}
