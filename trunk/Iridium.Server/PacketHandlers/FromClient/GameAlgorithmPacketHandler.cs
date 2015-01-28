namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;

    using Iridium.Network;
    using Iridium.Server.Games;
    using Iridium.Server.Services;
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;

    using IridiumDatabase;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class GameAlgorithmPacketHandler : PacketHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GameAlgorithmPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Start process GameAlgorithm.");

            //if (this.Client.State == SessionState.NotLogged)
            //{
            //    Logger.Error("Client is not logged");
            //    this.Disconnect();
            //    return;
            //}

            PacketsFromClient.GameAlgorithm gameAlgorithm = (PacketsFromClient.GameAlgorithm) this.Packet;
           
            level_data levelData;
            using (var db = new iridiumDB(Program.ConnectionString))
            {
                var query = from q in db.level_data
                            where q.game_id == (uint)gameAlgorithm.GameId
                                  && q.level_id == (uint)gameAlgorithm.LevelId
                            select q;
                levelData = query.First();
            }

            var data = LevelsDataProvider.GetLevelData(levelData);
            var json = JsonConvert.DeserializeObject<JToken>(data);

            var game = GamesFactory.GetGame(json["game_id"].ToObject<int>());

            string code = "Console.WriteLine(container.Enemies.Length);" +
                          "return 0;";
            string[] output = null;

            game.RunCode(json["input"].ToString(), code, out output);

            bool isSuccess = false;
          
//            this.Client.SendPacket(new PacketsFromMaster.AlgorithmResult(gameAlgorithm.GameId, gameAlgorithm.LevelId, output, isSuccess));
        }

        public void Run()
        {
            this.ProcessPacket();
        }
    }
}
