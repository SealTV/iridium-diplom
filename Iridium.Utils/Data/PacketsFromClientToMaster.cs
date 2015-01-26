namespace Iridium.Utils.Data
{
    using System;

    public static class PacketsFromClient
    {
        [Serializable]
        public class Ping : Packet
        {
            public int Value { get; private set; }

            public Ping(int value)
                            : base(ClientPacketType.Ping)
            {
                this.Value = value;
            }
        }

        [Serializable]
        public class Login : Packet
        {
            public string LoginName { get; private set; }
            public byte[] Password { get; private set; }

            public Login(string loginName, byte[] password) : base(ClientPacketType.Login)
            {
                this.LoginName = loginName;
                this.Password = password;
            }
        }

        [Serializable]
        public class GetGames : Packet
        {
            public GetGames() : base(ClientPacketType.GetGames)
            {}
        }

        [Serializable]
        public class GetGameData : Packet
        {
            public int GameId { get; private set; }


            public GetGameData(int gameId)
                            : base(ClientPacketType.GetGameData)
            {
                this.GameId = gameId;
            }
        }

        [Serializable]
        public class GetLevelData : Packet
        {
            public int GameId { get; private set; }
            public int LevelId { get; private set; }

            public GetLevelData(int gameId, int levelId)
                            : base(ClientPacketType.GetLevelData)
            {
                this.GameId = gameId;
                this.LevelId = levelId;
            }
        }

        [Serializable]
        public class GameAlgorithm : Packet
        {
            public int GameId { get; private set; }
            public int LevelId { get; private set; }
            public string Algorithm { get; private set; }

            public GameAlgorithm(int gameId, int levelId, string algoritm)
                            : base(ClientPacketType.GameAlgorithm)
            {
                this.GameId = gameId;
                this.LevelId = levelId;
                this.Algorithm = algoritm;
            }
        }
    }
}

