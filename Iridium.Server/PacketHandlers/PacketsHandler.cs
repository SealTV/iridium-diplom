using Iridiun.Utils.Data;

namespace Iridium.Server.PacketHandlers
{
    public abstract class PacketsHandler
    {

        public abstract void ProcessPacket(Packet packet);

        public void AddResponse(Packet packet)
        {
            
        }
    }
}
