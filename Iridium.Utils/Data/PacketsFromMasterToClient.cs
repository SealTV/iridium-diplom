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
        public int ServerVersion { get { return this.serverVersion; } }

        private readonly Guid clientId;
        public Guid ClientId { get { return this.clientId; } }

        public ServerInfo(Guid clientId)
            : base(MasterServerPacketType.ServerInfo)
        {
            this.serverVersion = 1;
            this.clientId = clientId;
        }
    }


    [Serializable]
    public class GameList : Packet
    {
        private readonly int[] gamesId;
        private int[] GamesId
        {
            get { return this.gamesId; }
        }

        public GameList(int[] gamesId)
            : base(MasterServerPacketType.GameList)
        {
            this.gamesId = gamesId;
        }
    }

    [Serializable]
    public class GameLevels : Packet
    {
        private readonly int gameId;
        public int GameId
        {
            get { return this.gameId; }
        }

        private readonly int[] levelsIds;
        private int[] LevelsIds
        {
            get { return this.levelsIds; }
        }

        public GameLevels(int gameId, int[] levelsIds)
            : base(MasterServerPacketType.GameLevels)
        {
            this.gameId = gameId;
            this.levelsIds = levelsIds;
        }
    }

    [Serializable]
    public class LevelData : Packet
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

        private readonly byte[] data;
        public byte[] Data
        {
            get { return this.data; }
        }



        public LevelData(int gameId, int levelId, byte[] data)
            : base(MasterServerPacketType.LevelData)
        {
            this.gameId = gameId;
            this.levelId = levelId;
            this.data = data;
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

        private readonly string message;
        public string Message
        {
            get { return this.message; }
        }

        public AlgorithmResult(int gameId, int levelId, string message)
            : base(MasterServerPacketType.AlgorithmResult)
        {
            this.gameId = gameId;
            this.levelId = levelId;
            this.message = message;
        }
    }
}
