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
            this.Client = this.Client;


            Ping ping = this.Packet as Ping;
            if (this.Packet == null)
            {
                Logger.Error("Cannot cast packet to Ping packet type.");
            }

            Logger.Info("Ping value = {0}", ping.Value);
        }
    }
}
