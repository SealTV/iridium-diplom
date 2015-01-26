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
            using (var db = new iridiumDB())
            {
                var query = from q in db.games
                            select q;
                games = query.ToList();
            }
            var gamesData = new GameData[games.Count];
            for (int i = 0; i < games.Count; i++)
            {
                gamesData[i] = new GameData((int)games[i].id, games[i].name, (int)games[i].levels_count);
            }

            Client.SendPacket(new GamesDataPacket(gamesData));
        }
    }
}