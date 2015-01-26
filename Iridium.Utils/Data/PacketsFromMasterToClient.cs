using System;

namespace Iridium.Utils.Data
{
    public static class PacketsFromMaster
    {
        [Serializable]
        public class Pong : Packet
        {
            public int Value { get; private set; }

            public Pong(int value)
                            : base(MasterServerPacketType.Pong)
            {
                this.Value = value;
            }
        }

        [Serializable]
        public class ServerInfo : Packet
        {
            public int ServerVersion { get; private set; }

            public Guid ClientId { get; private set; }

            public ServerInfo(Guid clientId)
                : base(MasterServerPacketType.ServerInfo)
            {
                this.ServerVersion = 1;
                this.ClientId = clientId;
            }
        }
        
        [Serializable]
        public class RegisterResult : Packet
        {
            public int AccountId { get; private set; }
            public bool RegisterOK { get; private set; }

            public RegisterResult(bool registerOK, int accountId)
                            : base(MasterServerPacketType.RegisterResult)
            {
                this.RegisterOK = registerOK;
                this.AccountId = accountId;
            }
        }

        [Serializable]
        public class LoginOk : Packet
        {
            public LoginOk()
                : base(MasterServerPacketType.LoginOk)
            {}
        }

        [Serializable]
        public class GamesList : Packet
        {
            public SharedPackets.GameData[] Games { get; private set; }

            public GamesList(SharedPackets.GameData[] games)
                            : base(MasterServerPacketType.GamesList)
            {
                this.Games = games;
            }
        }

        [Serializable]
        public class GameData : Packet
        {
            public int GameId { get; private set; }
            public string Name { get; private set; }
            public int CompletedLevels { get; private set; }
            public int LevelsCount { get; private set; }
            public string[] Levels { get; private set; }

            public GameData(int gameId, string name, int completedLevels, int levelsCount, string[] levels)
                : base(MasterServerPacketType.GameData)
            {
                this.GameId = gameId;
                this.Name = name;
                this.CompletedLevels = completedLevels;
                this.LevelsCount = levelsCount;
                this.Levels = levels;
            }
        }

        [Serializable]
        public class LevelData : Packet
        {

            public int GameId { get; private set; }
            public int LevelId { get; private set; }
            public byte[] Data { get; private set; }

            public LevelData(int gameId, int levelId, byte[] data)
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
}
