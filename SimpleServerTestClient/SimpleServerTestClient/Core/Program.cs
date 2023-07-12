using SimpleServerTestClient.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerTestClient.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServerAPI.Start("B");
            while (true)
            {
                if (ServerAPI.GetClientData(out GetData getData))
                {
                    Console.WriteLine(getData.GetString());
                }
            }
        }
    }
}
