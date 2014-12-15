namespace Iridium.Utils.Data
{
    using System;

    [Serializable]
    public abstract class Packet
    {
        public readonly Enum PacketType;

        protected Packet(Enum packetType)
        {
            this.PacketType = packetType;
        }
    }
    
    //From client packet types
    public enum ClientPacketType
    {
        Ping,
        GetGames,
        GetLevels,
        GetLevelData,
        GameAlgorithm
    }

    //From server packet types
    public enum MasterServerPacketType
    {
        Pong,
        ServerInfo,
        GameList,
        GameLevels,
        LevelData,
        AlgorithmResult
    }
}
