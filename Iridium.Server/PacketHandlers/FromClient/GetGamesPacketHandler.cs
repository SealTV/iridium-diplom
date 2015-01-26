namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;
    using System.Collections.Generic;

    using IridiumDatabase;
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;
    
    public class GetGamesPacketHandler : PacketHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GetGamesPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}
        
        protected override void ProcessPacket()
        {
            Logger.Info("Start process GetGamesPacke.");

            List<game> games;
            using (var db = new iridiumDB(Program.ConnectionString))
            {
                var query = from q in db.games
                            select q;
                games = query.ToList();
            }
            var gamesData = new SharedPackets.GameData[games.Count];
            for (int i = 0; i < games.Count; i++)
            {
                gamesData[i] = new SharedPackets.GameData((int)games[i].id, games[i].name);
            }

            Client.SendPacket(new PacketsFromMaster.GamesList(gamesData));
        }
    }
}