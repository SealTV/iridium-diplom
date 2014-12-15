namespace Iridium.Server.PacketHandlers.FromClient
{
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;
    
    public class GetGamesPacketHandler : PacketHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GetGamesPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}

        public override void ProcessPacket()
        {
            throw new System.NotImplementedException();
        }
    }
}
