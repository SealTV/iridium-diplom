﻿using System;

namespace Iridium.Utils.Data
{
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
        ClientPacket,
        MasterPacket,
        Ping,
        Pong,
        ServerInfo
    }
}
