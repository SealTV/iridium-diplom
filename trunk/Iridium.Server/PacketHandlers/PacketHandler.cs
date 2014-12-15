using Iridium.Server.Protocol;

namespace Iridium.Server.PacketHandlers
{
    using Iridium.Utils;
    using Iridium.Utils.Data;

    public abstract class PacketHandler
    {
        protected NetworkClient client;
        private readonly IridiumGameMasterServer masterServer;
        protected readonly Packet packet;

        protected PacketHandler(IridiumGameMasterServer masterServer, Packet packet)
        {
            this.masterServer = masterServer;
            this.packet = packet;
        }

        public void Run(NetworkClient client)
        {
            this.client = client;
            ProcessPacket();
        }

        public void Run()
        {
            this.ProcessPacket();

            this.EndProcessPacket();
        }

        public abstract void ProcessPacket();

        private void EndProcessPacket()
        {
            this.masterServer.AddClient(this.client);
        }

        protected void Disconnect()
        {
            this.client.Disconnect();
        }
    }
}
