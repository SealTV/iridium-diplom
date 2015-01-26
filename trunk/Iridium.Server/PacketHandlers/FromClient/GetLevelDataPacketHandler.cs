namespace Iridium.Server.PacketHandlers.FromClient
{
    using System.Linq;
    using System.Text;

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

            GetLevelDataPacket getLevelDataPacket = (GetLevelDataPacket) this.Packet;

            level_data levelData = null;
            using (var db = new iridiumDB())
            {
                var query = from q in db.level_data
                            where q.game_id == getLevelDataPacket.GameId
                                  && q.level_id == getLevelDataPacket.LevelId
                            select q;
                levelData = query.First();
            }
            var data = LavelsDataProvider.GetLevelData(levelData);
            this.Client.SendPacket(new LevelDataPacket((int)levelData.game_id, (int)levelData.level_id, Encoding.Unicode.GetBytes(data)));
        }
    }
}
