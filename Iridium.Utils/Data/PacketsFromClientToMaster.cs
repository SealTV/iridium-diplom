namespace Iridium.Utils.Data
{
    using System;

    [Serializable]
    public class Ping : Packet
    {
        private readonly int value;
        public int Value
        {
            get { return this.value; }
        }

        public Ping(int value)
            : base(ClientPacketType.Ping)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class GetGamesPacket : Packet
    {
        public GetGamesPacket()
            : base(ClientPacketType.GetGames)
        {

        }
    }

    [Serializable]
    public class GetGameDataPacket : Packet
    {
        private readonly int gameId;
        public int GameId
        {
            get { return this.gameId; }
        }

        public GetGameDataPacket(int gameId)
            : base(ClientPacketType.GetLevels)
        {
            this.gameId = gameId;
        }
    }

    [Serializable]
    public class GetLevelDataPacket : Packet
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

        public GetLevelDataPacket(int gameId, int levelId)
            : base(ClientPacketType.GetLevelData)
        {
            this.gameId = gameId;
            this.levelId = levelId;
        }
    }

    [Serializable]
    public class GameAlgorithmPacket : Packet
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
        private readonly string algoritm;
        public string Algoritm
        {
            get { return this.algoritm; }
        }

        public GameAlgorithmPacket(int gameId, int levelId, string algoritm)
            : base(ClientPacketType.GameAlgorithm)
        {
            this.gameId = gameId;
            this.levelId = levelId;
            this.algoritm = algoritm;
        }
    }
}

