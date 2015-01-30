namespace Iridium.Server.Protocol
{
    using System;

    using Iridium.Network;

    using Utils.Data;

    using PacketHandlers;
    using PacketHandlers.FromClient;

    public class IridiumMasterClientProtocol
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static IridiumMasterClientProtocol ClientProtocolHandler { get; private set; }

        private readonly IridiumGameMasterServer IridiumGameMasterServer;

        private IridiumMasterClientProtocol(IridiumGameMasterServer iridiumGameMasterServer)
        {
            this.IridiumGameMasterServer = iridiumGameMasterServer;
        }
        
        public static void Init(IridiumGameMasterServer iridiumGameMasterServer)
        {
            ClientProtocolHandler = new IridiumMasterClientProtocol(iridiumGameMasterServer);
        }

        public static void Stop()
        {
            ClientProtocolHandler = null;
        }

        public async void HandleNextClientAsync(NetworkClient client)
        {
            try
            {
                if (!client.SocketConnected())
                {
                    client.Disconnect();
                    return;
                }
                Packet packet = await client.ReadNextPacket();
                if (packet == null)
                {
                    IridiumGameMasterServer.AddClient(client);
                    return;
                }
                var packetHandler = GetPacketHandlerFor(packet);
                packetHandler.Handle(client);
            }
            catch (Exception e)
            {
                Logger.Error(e);
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
                case ClientPacketType.Register:
                    return new RegisterPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.Login:
                    return new LoginPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GetGames:
                    return new GetGamesPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GetGameData:
                    return new GetGameDataPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GetLevelData:
                    return new GetLevelDataPacketHandler(this.IridiumGameMasterServer, packet);
                case ClientPacketType.GameAlgorithm:
                    return new GameAlgorithmPacketHandler(this.IridiumGameMasterServer, packet);
                default:
                    throw new Exception("Unknown packet type");
            }
        }
    }
}
