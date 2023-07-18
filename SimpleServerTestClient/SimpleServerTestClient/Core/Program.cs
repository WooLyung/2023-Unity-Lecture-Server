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
            GetData getData;
            if (!ServerAPI.GetClientData(out getData))
                Console.WriteLine("!");
        }
    }
}
