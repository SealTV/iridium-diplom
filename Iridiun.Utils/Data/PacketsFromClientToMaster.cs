using System;

namespace Iridiun.Utils.Data
{
    [Serializable]
    public class Ping : Packet
    {
        private int ping;

        public Ping(PacketType packetType, Ping ping) : base(packetType)
        {}
    }
}
