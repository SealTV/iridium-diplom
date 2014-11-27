namespace Iridium.Server.Protocol
{
    using System;
    using Utils.Data;
    using PacketHandlers;
    using PacketHandlers.FromClient;

    public class IridiumMasterClientProtocol
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IridiumGameMasterServer IridiumGameMasterServer;

        public static IridiumMasterClientProtocol ClientProtocolHandler { get; private set; }

        public static void Init(IridiumGameMasterServer iridiumGameMasterServer)
        {
            ClientProtocolHandler = new IridiumMasterClientProtocol(iridiumGameMasterServer);
        }

        public static void Stop()
        {
            ClientProtocolHandler = null;
        }


        private IridiumMasterClientProtocol(IridiumGameMasterServer iridiumGameMasterServer)
        {
            this.IridiumGameMasterServer = iridiumGameMasterServer;
        }

        public async void HandleNextClient(Client client)
        {
            try
            {
                Packet packet = await client.ReceiveNextPacket();
                if (packet == null)
                {
                    logger.Warn("Received packet is null! Error!!!");
                    IridiumGameMasterServer.AddClient(client);
                    return;
                }
                var packetHandler = GetPacketHandlerFor(packet);
                if (packetHandler == null)
                {
                    logger.Warn("Packet handler for packet type {0} not found! 404 O_o", packet.PacketType.ToString());
                    IridiumGameMasterServer.AddClient(client);
                    return;                    
                }
                packetHandler.ProcessPacket(packet);
            }
            catch (Exception e)
            {
                logger.Error(e);
                IridiumGameMasterServer.Disconnect(client);
            }
            IridiumGameMasterServer.AddClient(client);
        }


        private PacketsHandler GetPacketHandlerFor(Packet packet)
        {
            switch (packet.PacketType)
            {
                case PacketType.Ping: return new PingPacketHandler();
                default: return null;
            }
        }
    }
}
