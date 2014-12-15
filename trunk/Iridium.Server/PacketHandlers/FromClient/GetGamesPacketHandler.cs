﻿namespace Iridium.Server.PacketHandlers.FromClient
{
    using Iridium.Server.Protocol;
    using Iridium.Utils;
    using Iridium.Utils.Data;
    
    public class GetGamesPacketHandler : PacketHandler
    {
        public GetGamesPacketHandler(IridiumGameMasterServer masterServer) : base(masterServer)
        {}

        public override bool ProcessPacket(NetworkClient client, Packet packet)
        {
            throw new System.NotImplementedException();
        }
    }
}
