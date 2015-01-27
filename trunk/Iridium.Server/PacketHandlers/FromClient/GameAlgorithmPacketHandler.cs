namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;

    using Iridium.Network;
    using Iridium.Server.Services;
    using Iridium.Utils.Data;
    using Iridium.Server.Protocol;

    using IridiumDatabase;

    public class GameAlgorithmPacketHandler : PacketHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            using (var db = new iridiumDB(Program.ConnectionString))
            {
                var query = from q in db.level_data
                            where q.game_id == (uint)gameAlgorithm.GameId
                                  && q.level_id == (uint)gameAlgorithm.LevelId
                            select q;
                levelData = query.First();
            }

            var data = LavelsDataProvider.GetLevelData(levelData);

            string output = string.Empty;
            bool isSuccess = false;
            this.Client.SendPacket(new PacketsFromMaster.AlgorithmResult(gameAlgorithm.GameId, gameAlgorithm.LevelId, output, isSuccess));
        }
    }
}
