using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVS_Server.Event.Inter
{
    public class InterEvent_Damage : InterEvent
    {
        public int id;
        public int victim;
        public int damage;
    }
}
