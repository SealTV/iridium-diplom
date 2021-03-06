﻿namespace Iridium.Utils.Data
{
    using System;

    [Serializable]
    public abstract class Packet
    {
        public Enum PacketType { get; private set; }

        protected Packet(Enum packetType)
        {
            this.PacketType = packetType;
        }
    }
    
    //From client packet types
    public enum ClientPacketType
    {
        Ping,
        Register,
        Login,
        GetGames,
        GetGameData,
        GetLevelData,
        GameAlgorithm
    }

    //From server packet types
    public enum MasterServerPacketType
    {
        Pong,
        ServerInfo,
        RegisterResult,
        LoginOk,
        GamesList,
        GameData,
        LevelData,
        AlgorithmResult
    }

}
