namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Iridium.Network;
    using Iridium.Server.Games;
    using Iridium.Server.Services;
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;

    using IridiumDatabase;

    using LinqToDB;
    
    public class GameAlgorithmPacketHandler : PacketHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GameAlgorithmPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Start process GameAlgorithm.");

            if (this.Client.State == SessionState.NotLogged)
            {
                Logger.Error("Client is not logged");
                this.Disconnect();
                return;
            }

            PacketsFromClient.GameAlgorithm gameAlgorithm = (PacketsFromClient.GameAlgorithm) this.Packet;
           
            level_data levelData;
            using (var db = new iridiumDB(IridiumMasterServer.ConnectionString))
            {
                var query = from q in db.level_data
                            where q.game_id == (uint)gameAlgorithm.GameId && q.level_id == (uint)gameAlgorithm.LevelId
                            select q;
                levelData = query.First();
            }

            var data = LevelsDataProvider.GetLevelData(levelData);
            var json = JsonConvert.DeserializeObject<JToken>(data);

            var game = GamesFactory.GetGame(json["game_id"].ToObject<int>());

            string[] output;
            bool isSuccess = game.RunCode(json["input"].ToString(), gameAlgorithm.Algorithm, out output);

            if (isSuccess)
            {
                using (var db = new iridiumDB(IridiumMasterServer.ConnectionString))
                {
                    var compleated_level = (from q in db.completed_levels
                                            where q.account == this.Client.AccountId && q.game == levelData.game_id
                                            select q).FirstOrDefault();
    
                    if (compleated_level != null)
                    {
                        if (compleated_level.levels_ccomplete < levelData.level_id)
                            compleated_level.levels_ccomplete = levelData.level_id;
                        db.Update(compleated_level);
                    }
                    else
                        db.Insert(new completed_levels
                        {
                            account = this.Client.AccountId,
                            game = levelData.game_id,
                            levels_ccomplete = levelData.level_id
                        });
                }
            }

            this.Client.SendPacket(new PacketsFromMaster.AlgorithmResult(gameAlgorithm.GameId, gameAlgorithm.LevelId, output, isSuccess));
        }
    }
}
