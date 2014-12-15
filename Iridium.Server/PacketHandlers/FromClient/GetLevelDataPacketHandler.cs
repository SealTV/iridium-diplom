namespace Iridium.Server.PacketHandlers.FromClient
{
    using Iridium.Server.Protocol;
    using Iridium.Utils.Data;
    
    public class GetLevelDataPacketHandler : PacketHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GetLevelDataPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}

        public override void ProcessPacket()
        {
            throw new System.NotImplementedException();
        }
    }
}
