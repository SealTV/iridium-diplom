using Iridium.Utils.Data;

using NLog;

namespace Iridium.Server.PacketHandlers.FromClient
{
    public class PingPacketHandler : PacketsHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public override void ProcessPacket(Packet packet)
        {
            Ping ping = packet as Ping;
            if (packet == null)
            {
                logger.Error("Cannot cast packet to Ping packet type.");
            }

            logger.Info("Ping value = {0}", ping.Value);
        }
    }
}
