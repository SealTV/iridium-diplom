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
        public class LoginResult : Packet
        {
            public LoginResults Result { get; private set; }

            public LoginResult(LoginResults result)
                            : base(MasterServerPacketType.LoginOk)
            {
                this.Result = result;
            }
        }

        [Serializable]
        public class GamesList : Packet
        {
            public SharedData.GameData[] Games { get; private set; }

            public GamesList(SharedData.GameData[] games)
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
            public string InputParameters { get; private set; }

            public LevelData(int gameId, int levelId, string inputParameters)
                            : base(MasterServerPacketType.LevelData)
            {
                this.InputParameters = inputParameters;
                this.LevelId = levelId;
                this.GameId = gameId;
            }
        }

        [Serializable]
        public class AlgorithmResult : Packet
        {
            public int GameId { get; private set; }
            public int LevelId { get; private set; }
            public string[] Output { get; private set;}
            public bool IsSuccess { get; private set;}

            public AlgorithmResult(int gameId, int levelId, string[] output, bool isSuccess)
                            : base(MasterServerPacketType.AlgorithmResult)
            {
                this.GameId = gameId;
                this.LevelId = levelId;
                this.Output = output;
                this.IsSuccess = isSuccess;
            }
        }
    }
}
