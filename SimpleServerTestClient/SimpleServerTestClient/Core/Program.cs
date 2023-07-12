using SimpleServerAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerAPI.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServerAPI.Start("GroupName");
            GetData getData = ServerAPI.GetServerData("MyKey");
            if (getData == null)
            {
                ServerAPI.PostServerData("MyKey", new PostData().AddInt(0));
            }
            else
            {
                int value = getData.GetInt();
                ServerAPI.PostServerData("MyKey", new PostData().AddInt(value + 1));

                Console.WriteLine(value);
            }
        }
    }
}
