using System;
using Iridium.Server.Protocol;
using Iridium.Utils.Data;

namespace Iridium.Server.PacketHandlers
{
    public abstract class PacketsHandler : IDisposable
    {
        public IridiumGameMasterServer MasterServer;
        public Client Client;

        public abstract void ProcessPacket(Packet packet);

        public void AddResponse(Packet packet)
        {
            Client.SendPacket(packet);   
        }

        public void Dispose()
        {
            MasterServer = null;
            Client = null;
        }
    }
}
