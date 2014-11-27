using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Iridium.Utils.Data;

namespace Iridium.Server
{
    public class Client : IDisposable
    {
        // Client  socket.
        private readonly Socket socket;

        readonly BinaryFormatter formater = new BinaryFormatter();

        public bool isLogin;

        private readonly NetworkStream workStream;

        public Client(Socket socket)
        {
            this.socket = socket;
            this.workStream = new NetworkStream(socket);
        }

        public void SendPacket(Packet packet)
        {
            this.formater.Serialize(this.workStream, packet);
        }

        public bool TryGetPacket(out Packet packet)
        {
            if (workStream.DataAvailable)
            {
                packet = this.formater.Deserialize(this.workStream) as Packet;
                return true;
            }
            packet = null;
            return false;
        }

        public Task<Packet> ReceiveNextPacket()
        {
            return Task<Packet>.Factory.StartNew(() => this.formater.Deserialize(this.workStream) as Packet);
        }

        public void Disconnect()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.workStream.Dispose();
            this.socket.Dispose();
        }
    }
}
