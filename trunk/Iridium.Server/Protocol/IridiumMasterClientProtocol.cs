namespace Iridium.Server.Protocol
{
    using System;
    using Utils;
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

        public async void 
            HandleNextClient(NetworkClient client)
        {
            try
            {
                Packet packet = await client.ReadNextPacketAsync();
                if (packet == null)
                {
                    logger.Warn("Received packet is null! Error!!!");
                    client.Disconnect();
                    //IridiumGameMasterServer.AddClient(client);
                    return;
                }
                var packetHandler = GetPacketHandlerFor(packet);
                if (packetHandler == null)
                {
                    logger.Warn("Packet handler for packet type {0} not found! 404 O_o", packet.PacketType.ToString());
                    IridiumGameMasterServer.AddClient(client);
                    return;                    
                }
                packetHandler.ProcessPacket();
            }
            catch (Exception e)
            {
                logger.Error(e);
                IridiumGameMasterServer.Disconnect(client);
            }
        }


        private PacketHandler GetPacketHandlerFor(Packet packet)
        {
            if (!(packet.PacketType is ClientPacketType))
            {
                return null;
            }

            switch ((ClientPacketType) packet.PacketType)
            {
                case ClientPacketType.Ping:
                    return new PingPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GetGames:
                    return new GetGamesPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GetLevels:
                    return new GetLevelsPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GetLevelData:
                    return new GetLevelDataPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GameAlgorithm:
                    return new GameAlgorithmPacketHandler(this.IridiumGameMasterServer, packet);
                default:
                    return null;
            }
        }
    }
}
