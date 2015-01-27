namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;
    using System.Text;
    using Iridium.Network;
    using Iridium.Server.Protocol;
    using Iridium.Utils;
    using Iridium.Utils.Data;

    using IridiumDatabase;

    public class LoginPacketHandler : PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public LoginPacketHandler(IridiumGameMasterServer masterServer, Packet packet) : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Strart process Login packet.");

            if (this.Client.State == SessionState.LoggedIn)
            {
                Logger.Error("logginned client reloggin?! Error!");
                this.Disconnect();
                return;
            }
            
            if (this.Packet == null)
            {
                Logger.Error("Cannot cast packet to Ping packet type.");
            }
            PacketsFromClient.Login login = (PacketsFromClient.Login) this.Packet;

            Logger.Info("Login name = {0}, password = {1}", login.LoginName, Encoding.Unicode.GetString(login.Password));

            using (var db = new iridiumDB(Program.ConnectionString))
            {
                account account = (from a in db.accounts
                                   where a.login == login.LoginName
                                         && Enumerable.SequenceEqual(a.password, login.Password)
                                   select a).First();
                if (account != null)
                {
                    this.Client.SendPacket(new PacketsFromMaster.LoginOk());
                    this.Client.AccountId = account.id;
                }
            }
            

        }
    }
}
