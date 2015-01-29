
namespace Iridium.Client.PacketHandlers
{
    using Assets.Scripts;
    using Iridium.Network;
    using Iridium.Utils.Data;

    public abstract class PacketHandler
    {
        protected NetworkClient Client;
        protected readonly Packet Packet;
        protected IServerConnector ServerConnector;

        protected PacketHandler(Packet packet,IServerConnector serverConnector)
        {
            this.ServerConnector = serverConnector;
            this.Packet = packet;
        }

        public void Handle(NetworkClient client)
        {
            this.Client = client;
            this.ProcessPacket();
        }

        protected abstract void ProcessPacket();

        protected void Disconnect()
        {
            this.Client.Disconnect();
        }

        public class GameDataPacketHandler:PacketHandler
        {
            public GameDataPacketHandler(Packet packet, IServerConnector serverConnector) : base(packet, serverConnector)
            {
            }

            protected override void ProcessPacket()
            {

                //ServerConnector.On
            }
        }

        public class GameListPacketHandler : PacketHandler
        {
            public GameListPacketHandler(Packet packet, IServerConnector serverConnector) : base(packet, serverConnector)
            {
            }

            protected override void ProcessPacket()
            {
                PacketsFromMaster.GamesList pak = (PacketsFromMaster.GamesList) Packet;
            }
        }
    }
}
