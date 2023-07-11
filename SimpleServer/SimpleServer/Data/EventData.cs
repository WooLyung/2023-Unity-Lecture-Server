using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer.Data
{
    public class EventData
    {
        public String Group
        {
            private set;
            get;
        }

        public Byte[] ByteData
        {
            private set;
            get;
        }

        public EventData(String group, Byte[] data)
        {
            this.Group = group;
            this.ByteData = data;
        }
    }
}
