namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;

    using Iridium.Network;
    using Iridium.Server.Protocol;
    using Iridium.Server.Services;
    using Iridium.Utils.Data;

    using IridiumDatabase;



    public class GetLevelDataPacketHandler : PacketHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public GetLevelDataPacketHandler(IridiumGameMasterServer masterServer, Packet packet)
            : base(masterServer, packet)
        {}

        protected override void ProcessPacket()
        {
            Logger.Info("Start process GetLevelData.");

            if (this.Client.State == SessionState.NotLogged)
            {
                Logger.Error("Client is not logged");
                this.Disconnect();
                return;
            }

            PacketsFromClient.GetLevelData getLevelData = (PacketsFromClient.GetLevelData) this.Packet;

            level_data levelData;
            using (var db = new iridiumDB(IridiumMasterServer.ConnectionString))
            {
                var query = from q in db.level_data
                            where q.game_id == (uint)getLevelData.GameId
                                  && q.level_id == (uint)getLevelData.LevelId
                            select q;
                levelData = query.First();
            }

            var data = LevelsDataProvider.GetLevelData(levelData);
            this.Client.SendPacket(new PacketsFromMaster.LevelData((int)levelData.game_id, (int)levelData.level_id, data));
        }
    }
}
