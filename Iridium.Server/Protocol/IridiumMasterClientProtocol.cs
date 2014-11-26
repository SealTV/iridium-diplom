using System;

using Iridiun.Utils.Data;

using Iridium.Server.PacketHandlers;
using Iridium.Server.PacketHandlers.FromClient;
namespace Iridium.Server.Protocol
{
    public class IridiumMasterClientProtocol
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static IridiumMasterClientProtocol iridiumMasterClientProtocol;


        public static void Init(IridiumGameMasterServer iridiumGameMasterServer)
        {
            iridiumMasterClientProtocol = new IridiumMasterClientProtocol(iridiumGameMasterServer);
        }

        public static void Stop()
        {
            iridiumMasterClientProtocol = null;
        }

        private readonly IridiumGameMasterServer IridiumGameMasterServer;

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
                    IridiumGameMasterServer.SetClient(client);
                    return;
                }
                var packetHandler = GetPacketHandlerFor(packet);
                if (packetHandler == null)
                {
                    logger.Warn("Packet handler for packet type {0} not found! 404 O_o", packet.PacketType.ToString());
                    IridiumGameMasterServer.SetClient(client);
                    return;                    
                }
                packetHandler.ProcessPacket(packet);
            }
            catch (Exception e)
            {
                logger.Error(e);
                IridiumGameMasterServer.Disconnect(client);
            }
            IridiumGameMasterServer.SetClient(client);
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
