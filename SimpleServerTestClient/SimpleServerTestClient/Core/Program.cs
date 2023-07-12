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

            ServerAPI.PostServerData("X", new PostData()
                .AddIntArray(new int[] { 1, 2, 3, 4 }));

            GetData getData = ServerAPI.GetServerData("X");
            int[] v = getData.GetIntArray();
            foreach (int i in v)
                Console.WriteLine(i);
        }
    }
}
