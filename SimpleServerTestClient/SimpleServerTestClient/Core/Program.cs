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
            ServerAPI.Start("TestGroup");
            ServerAPI.SetServerData("AaAa", 1.414f);
            Console.WriteLine(
                ServerAPI.GetServerDataFloat("AaAa", 0.4f)
            );
        }
    }
}
