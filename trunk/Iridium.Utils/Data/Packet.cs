namespace Iridium.Utils.Data
{
    using System;

    [Serializable]
    public abstract class Packet
    {
        public Enum PacketType { get; protected set; }

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
        GamesData,
        GameLevelsData,
        LevelData,
        AlgorithmResult
    }

}
