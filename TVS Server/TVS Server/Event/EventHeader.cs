using System.Runtime.Serialization;

namespace TVS_Server.Event
{
    [Serializable]
    public class EventHeader : ISerializable
    {
        public string name;
    
        public EventHeader(string name)
        {
            this.name = name;
        }

        public EventHeader(SerializationInfo info, StreamingContext context)
        {
            name = info.GetString("name");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
        }
    }
}