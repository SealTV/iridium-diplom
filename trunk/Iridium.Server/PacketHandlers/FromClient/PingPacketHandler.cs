namespace Iridium.Server.PacketHandlers.FromClient
{
    using Protocol;

    using Utils.Data;

    public class PingPacketHandler : PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public PingPacketHandler(IridiumGameMasterServer masterServer, Packet packet) : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Strart process Ping packet.");
            if (this.Packet == null)
            {
                Logger.Error("Cannot cast packet to Ping packet type.");
            }
            PacketsFromClient.Ping ping = this.Packet as PacketsFromClient.Ping;
            
            Logger.Info("Ping value = {0}", ping.Value);
            this.Client.SendPacket(new PacketsFromMaster.Pong(ping.Value));
        }
    }
}
