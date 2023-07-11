namespace SimpleServer.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Start(8462);
        }
    }
}