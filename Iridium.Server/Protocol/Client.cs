using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Iridium.Utils.Data;

namespace Iridium.Server.Protocol
{
    public class Client : IDisposable
    {
        // Client  socket.
        private readonly Socket socket;

        readonly BinaryFormatter formatter = new BinaryFormatter();

        private readonly NetworkStream inputStream;
        private readonly NetworkStream outputStream;

        public Guid SessionId { get; private set; }

        public Client(Socket socket)
        {
            this.socket = socket;
            this.inputStream = new NetworkStream(socket, FileAccess.Read);
            this.outputStream = new NetworkStream(socket, FileAccess.Write);

            this.SessionId = Guid.NewGuid();
        }

        public void SendPacket(Packet packet)
        {
            this.formatter.Serialize(this.outputStream, packet);
        }

        public bool TryGetPacket(out Packet packet)
        {
            if (this.inputStream.DataAvailable)
            {
                packet = this.formatter.Deserialize(this.inputStream) as Packet;
                return true;
            }
            packet = null;
            return false;
        }

        public Task<Packet> ReceiveNextPacket()
        {
            return Task<Packet>.Factory.StartNew(() => this.formatter.Deserialize(this.inputStream) as Packet);
        }

        public void Disconnect()
        {
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Disconnect(false);

            this.Dispose();
        }

        public void Dispose()
        {
            this.inputStream.Dispose();
            this.outputStream.Dispose();
            this.socket.Dispose();
        }
    }
}
