using System;

namespace Iridium.Utils.Data
{
    [Serializable]
    public class Pong : Packet
    {
        private readonly int value;
        public int Value { get { return this.value; }}

        public Pong(int value)
            : base(MasterServerPacketType.Pong)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class ServerInfo : Packet
    {
        private readonly int serverVersion;
        public int ServerVersion
        {
            get { return this.serverVersion; }
        }

        private readonly Guid clientId;
        public Guid ClientId
        {
            get { return this.clientId; }
        }

        public ServerInfo(Guid clientId)
            : base(MasterServerPacketType.ServerInfo)
        {
            this.serverVersion = 1;
            this.clientId = clientId;
        }
    }

    [Serializable]
    public class GamesDataPacket : Packet
    {
        public GameData[] Games { get; private set; }

        public GamesDataPacket(GameData[] games)
            : base(MasterServerPacketType.GamesData)
        {
            this.Games = games;
        }
    }

    [Serializable]
    public class GameDataPacket : Packet
    {
        public int GameId { get; private set; }
        public LevelData[] LevelsIds { get; private set; }

        public GameDataPacket(int gameId, LevelData[] levelsIds)
            : base(MasterServerPacketType.GameLevelsData)
        {
            this.LevelsIds = levelsIds;
            this.GameId = gameId;
        }
    }

    [Serializable]
    public class LevelDataPacket : Packet
    {

        public int GameId { get; private set; }
        public int LevelId { get; private set; }
        public byte[] Data { get; private set; }

        public LevelDataPacket(int gameId, int levelId, byte[] data)
            : base(MasterServerPacketType.LevelData)
        {
            this.Data = data;
            this.LevelId = levelId;
            this.GameId = gameId;
        }
    }

    [Serializable]
    public class AlgorithmResult : Packet
    {
        private readonly int gameId;
        public int GameId
        {
            get { return this.gameId; }
        }

        private readonly int levelId;
        public int LevelId
        {
            get { return this.levelId; }
        }

        private readonly string[] steps;
        public string[] Steps
        {
            get { return this.steps; }
        }

        public AlgorithmResult(int gameId, int levelId, string[] steps)
            : base(MasterServerPacketType.AlgorithmResult)
        {
            this.gameId = gameId;
            this.levelId = levelId;
            this.steps = steps;
        }
    }
}
