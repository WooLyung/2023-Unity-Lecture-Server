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
       
            GetData getData = ServerAPI.GetServerData("XX");
            Console.WriteLine(getData.GetInt());
            Console.WriteLine(getData.GetFloat());
            Console.WriteLine(getData.GetString());

            getData = ServerAPI.GetServerData("XX");
            Console.WriteLine(getData.GetInt());
            Console.WriteLine(getData.GetFloat());
            Console.WriteLine(getData.GetString());

            getData = ServerAPI.GetServerData("XX");
            Console.WriteLine(getData.GetInt());
            Console.WriteLine(getData.GetFloat());
            Console.WriteLine(getData.GetString());
        }
    }
}
