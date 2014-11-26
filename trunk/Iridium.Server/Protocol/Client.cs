using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

using Iridiun.Utils.Data;

namespace Iridium.Server
{
    public class Client : IDisposable
    {
        // Client  socket.
        private Socket socket;

        readonly BinaryFormatter formater = new BinaryFormatter();

        private readonly NetworkStream inputStream;
        private readonly NetworkStream outputStream;

        public Client(Socket socket)
        {
            this.socket = socket;
            this.inputStream = new NetworkStream(socket);
            this.outputStream = new NetworkStream(socket);
        }

        public void SendPacket(Packet packet)
        {
            this.formater.Serialize(this.outputStream, packet);
        }

        public Task<Packet> ReceiveNextPacket()
        {
            return Task<Packet>.Factory.StartNew(() => this.formater.Deserialize(this.inputStream) as Packet);
        }

        public void Disconnect()
        {
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Dispose();
        }

        public void Dispose()
        {
            this.socket.Dispose();
            this.inputStream.Dispose();
            this.outputStream.Dispose();
        }
    }
}
