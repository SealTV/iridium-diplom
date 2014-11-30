using System;

namespace Iridium.Utils.Data
{
    [Serializable]
    public class Pong : Packet
    {
        public int Value { get; private set; }

        public Pong(int value) : base(PacketType.Pong)
        {
            this.Value = value;
        }
    }

    [Serializable]
    public class ServerInfo : Packet
    {
        public int ServerVersion { get; private set; }

        public Guid ClientId { get; private set; }

        public ServerInfo() : base(PacketType.ServerInfo)
        {
            ServerVersion = 1;
            ClientId = Guid.NewGuid();
        }
    }
}
