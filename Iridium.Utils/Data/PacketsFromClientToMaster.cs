using System;

namespace Iridium.Utils.Data
{
    [Serializable]
    public class Ping : Packet
    {
        private readonly int ping;

        public Ping(PacketType packetType, Ping ping) : base(packetType)
        {
            this.ping = ping.ping;
        }
    }
}
