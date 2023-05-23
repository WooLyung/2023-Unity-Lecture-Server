using System.Runtime.Serialization;
using TVS_Server.Event.Emit;

namespace TVS_Server.Event.On
{
    public class OnEvent_Join : OnEvent
    {
        public string nickname;
        public string color;

        public OnEvent_Join(byte[] buffer)
        {
            nickname = System.Text.Encoding.UTF8.GetString(buffer, 0, 16);
            color = System.Text.Encoding.UTF8.GetString(buffer, 16, 6);
        }
    }
}