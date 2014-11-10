using Iridium.Server.PacketHandlers;
using Iridium.Server.PacketHandlers.FromClient;

using Iridiun.Utils.Data;

namespace Iridium.Server.Protocol
{
    public class IridiumMasterClientProtocol
    {
        public PacketsHandler GetPacketHandlerFor(Packet packet)
        {
            switch (packet.PacketType)
            {
                case PacketType.Ping: return new PingPacketHandler();
                default: return null;
            }
        }
    }
}
