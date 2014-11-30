using System;

namespace Iridium.Utils.Data
{
    [Serializable]
    public class Ping : Packet
    {
        private readonly int ping;

        public int Value
        {
            get { return this.ping; }
        }

        public Ping(int value) : base(PacketType.Ping)
        {
            this.ping = value;
        }
    }
}
