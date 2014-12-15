using Iridium.Server.Protocol;

namespace Iridium.Server.PacketHandlers
{
    using Iridium.Utils;
    using Iridium.Utils.Data;

    public abstract class PacketHandler
    {
        protected NetworkClient Client;
        protected IridiumGameMasterServer masterServer;

        protected PacketHandler(IridiumGameMasterServer masterServer)
        {
            this.masterServer = masterServer;
        }

        public abstract bool ProcessPacket(NetworkClient client, Packet packet);
    }
}
