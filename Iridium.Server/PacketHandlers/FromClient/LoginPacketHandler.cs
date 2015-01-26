namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Text;

    using Iridium.Server.Protocol;
    using Iridium.Utils.Data;

    public class LoginPacketHandler : PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginPacketHandler(IridiumGameMasterServer masterServer, Packet packet) : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Strart process Login packet.");
            if (this.Packet == null)
            {
                Logger.Error("Cannot cast packet to Ping packet type.");
            }
            PacketsFromClient.Login login = (PacketsFromClient.Login) this.Packet;

            Logger.Info("Login name = {0}, password = {1}", login.LoginName, Encoding.Unicode.GetString(login.Password));
            this.Client.SendPacket(new PacketsFromMaster.LoginOk());
        }
    }
}
