namespace Iridium.Utils.Data
{
    using System;

    [Serializable]
    public abstract class Packet
    {
        public readonly PacketType PacketType;

        protected Packet(PacketType packetType)
        {
            this.PacketType = packetType;
        }
    }

    public enum PacketType
    {
        //From client packet types
        Ping,
        ServerInfo,
        GameAlgorithm,
        GetGames,
        GetLevels,
        GetLevelData,
      //From server packet types
        Pong,
        GameList,
        GameLevels,
        LevelData,
        AlgorithmResult
    }
}
