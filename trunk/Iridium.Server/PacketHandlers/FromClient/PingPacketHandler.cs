namespace Iridium.Server.PacketHandlers.FromClient
{
    using NLog;
    using Protocol;
    using Utils;
    using Utils.Data;

    public class PingPacketHandler : PacketHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public PingPacketHandler(IridiumGameMasterServer masterServer) : base()
        {}

        public override bool ProcessPacket(NetworkClient client, Packet packet)
        {
            this.Client = client;


            Ping ping = packet as Ping;
            if (packet == null)
            {
                logger.Error("Cannot cast packet to Ping packet type.");
            }

            logger.Info("Ping value = {0}", ping.Value);

            return true;
        }
    }
}
