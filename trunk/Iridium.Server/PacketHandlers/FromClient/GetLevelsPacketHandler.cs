using Iridium.Server.Protocol;
using Iridium.Utils;
using Iridium.Utils.Data;

namespace Iridium.Server.PacketHandlers.FromClient
{
    public class GetLevelsPacketHandler : PacketHandler
    {
        public GetLevelsPacketHandler(IridiumGameMasterServer masterServer) : base(masterServer)
        {}

        public override bool ProcessPacket(NetworkClient client, Packet packet)
        {
            throw new System.NotImplementedException();
        }
    }
}
