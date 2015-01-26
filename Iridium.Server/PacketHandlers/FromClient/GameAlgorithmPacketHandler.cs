namespace Iridium.Server.PacketHandlers.FromClient
{
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;

    public class GameAlgorithmPacketHandler : PacketHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GameAlgorithmPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Start process GameAlgorithm.");

        }
    }
}
