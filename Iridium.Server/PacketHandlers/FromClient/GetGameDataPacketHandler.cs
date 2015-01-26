namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;

    using Iridium.Server.Protocol;
    using Iridium.Utils.Data;

    using IridiumDatabase;

    public class GetGameDataPacketHandler : PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GetGameDataPacketHandler(IridiumGameMasterServer masterServer, Packet packet) : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Start process GetGemaLevels packet");
            PacketsFromClient.GetGameData getGameData = (PacketsFromClient.GetGameData) this.Packet;

            game game;
            completed_levels completedLevels;
            string[] levels;

            using (var db = new iridiumDB())
            {
                var games = from q in db.games
                            where q.id == getGameData.GameId
                            select q;
                game = games.First();

                var completedLevelses = from q in db.completed_levels
                             where q.game == game.id
                             select q;
                completedLevels = completedLevelses.First();

                levels = (from q in db.levels
                          where q.id == game.id
                          select q.name).ToArray();
            }

            PacketsFromMaster.GameData gameData = new PacketsFromMaster.GameData((int)game.id, game.name, (int)completedLevels.levels_ccomplete, (int)game.levels_count, levels);
            this.Client.SendPacket(gameData);
        }
    }
}
