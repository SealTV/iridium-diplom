namespace Iridium.Server.PacketHandlers.FromClient
{
    using Iridium.Server.Protocol;
    using Iridium.Utils.Data;

    public class GetLevelsPacketHandler : PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GetLevelsPacketHandler(IridiumGameMasterServer masterServer, Packet packet) : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            
        }
    }
}
