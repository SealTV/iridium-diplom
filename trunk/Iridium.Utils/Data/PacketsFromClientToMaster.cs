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
    public class GetGames : Packet
    {
        public GetGames()
            : base(ClientPacketType.GetGames)
        {}
    }

    [Serializable]
    public class GetLevels : Packet
    {
        private readonly int gameId;
        public int GameId
        {
            get { return this.gameId; }
        }

        public GetLevels(int gameId)
            : base(ClientPacketType.GetLevels)
        {
            this.gameId = gameId;
        }
    }

    [Serializable]
    public class GetLevelData : Packet
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

        public GetLevelData(int gameId, int levelId)
            : base(ClientPacketType.GetLevelData)
        {
            this.gameId = gameId;
            this.levelId = levelId;
        }
    }


    [Serializable]
    public class GameAlgorithm : Packet
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
        private readonly string message;
        public string Message
        {
            get { return this.message; }
        }

        public GameAlgorithm(int gameId, int levelId, string message)
            : base(ClientPacketType.GameAlgorithm)
        {
            this.gameId = gameId;
            this.levelId = levelId;
            this.message = message;
        }
    }
}

