using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVS_Server.Event.Inter
{
    public class InterEvent_Join : InterEvent
    {
        public int id;
        public string nickname;
        public string color;
        public float x;
        public float y;
        public float angle;
        public int hp;
    }
}
