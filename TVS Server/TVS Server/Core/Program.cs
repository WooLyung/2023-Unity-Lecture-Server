namespace TVS_Server.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Start(9172);
        }
    }
}