namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;
    using System.Text;

    using Iridium.Network;
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;

    using IridiumDatabase;

    using LinqToDB.SqlQuery;

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

            Logger.Info("Login name = {0}, password = {1}", login.LoginName, Encoding.UTF8.GetString(login.Password));

            try
            {
                using (var db = new iridiumDB(IridiumMasterServer.ConnectionString))
                {
                    account account = (from a in db.accounts
                                       where a.login == login.LoginName
                                       select a).First();
                    if (account != null)
                    {
                        if (Encoding.UTF8.GetString(account.password) == Encoding.UTF8.GetString(login.Password))
                        {
                            Logger.Info("Loggin success!");
                            this.Client.SendPacket(new PacketsFromMaster.LoginResult(LoginResults.LoginOk));
                            this.Client.AccountId = account.id;
                            this.Client.State = SessionState.LoggedIn;
                        }
                        else
                        {
                            Logger.Error("Incorrect password");
                            this.Client.SendPacket(new PacketsFromMaster.LoginResult(LoginResults.PasswordIncorrect));
                            this.Disconnect();    
                        }
                    }
                    else
                    {
                        Logger.Error("User with login name {0} are not found password", login.LoginName);
                        this.Client.SendPacket(new PacketsFromMaster.LoginResult(LoginResults.UserNotFount));
                        this.Disconnect();    
                    }
                }
            }
            catch (SqlException e)
            {
                Logger.Error(e);
                this.Client.SendPacket(new PacketsFromMaster.LoginResult(LoginResults.LoginFail));
                this.Disconnect();    
            }
        }
    }
}
