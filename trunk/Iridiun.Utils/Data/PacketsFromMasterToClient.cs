using System;

namespace Iridiun.Utils.Data
{
    [Serializable]
    public class Pong : Packet
    {
        private int pong;

        public Pong(PacketType packetType) : base(packetType)
        {}

    }
}
