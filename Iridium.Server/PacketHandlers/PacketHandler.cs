namespace Iridium.Server.PacketHandlers
{
    using System;

    using Iridium.Network;
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;

    public abstract class PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly IridiumGameMasterServer MasterServer;
       
        protected NetworkClient Client;
        protected readonly Packet Packet;

        protected PacketHandler(IridiumGameMasterServer masterServer, Packet packet)
        {
            this.MasterServer = masterServer;
            this.Packet = packet;
        }

        public void Handle(NetworkClient client)
        {
            this.Client = client;
            try
            {
                this.ProcessPacket();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                this.Disconnect();
            }

            this.EndProcessPacket();
        }

        protected abstract void ProcessPacket();

        private void EndProcessPacket()
        {
            this.MasterServer.AddClient(this.Client);
        }

        protected void Disconnect()
        {
            this.Client.Disconnect();
        }
    }
}
