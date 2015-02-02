namespace Iridium.Server.PacketHandlers.FromClient
{
    using System;
    using System.Linq;

    using Iridium.Server.Protocol;
    using Iridium.Utils.Data;

    using IridiumDatabase;

    using LinqToDB;

    public class RegisterPacketHandler : PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public RegisterPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}
        
        protected override void ProcessPacket()
        {
            Logger.Info("Strart process Register packet.");
            if (this.Packet == null)
            {
                Logger.Error("Cannot cast packet to Ping packet type.");
            }
            PacketsFromClient.Register register = (PacketsFromClient.Register)this.Packet;

            bool registerOK = false;
            int accountId = 0;
            using (var db = new iridiumDB(IridiumMasterServer.ConnectionString))
            {
                var query = (from q in db.accounts
                             where q.login == register.LoginName
                             select q).ToList();
                if (query.Count == 0)
                {
                    account account = new account()
                    {
                        login = register.LoginName,
                        password = register.Password,
                        date = DateTime.Now
                    };
                    accountId = db.Insert(account);
                    registerOK = true;
                }
            }

            this.Client.SendPacket(new PacketsFromMaster.RegisterResult(registerOK, accountId));
        }
    }
}
