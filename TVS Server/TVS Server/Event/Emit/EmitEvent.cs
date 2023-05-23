using TVS_Server.Util;

namespace TVS_Server.Event.Emit
{
    public abstract class EmitEvent : Decodable
    {
        public abstract byte[] ToBinary();
    }
}