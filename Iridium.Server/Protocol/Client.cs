using System.Net.Sockets;
using System.Text;

using Iridiun.Utils.Data;

namespace Iridium.Server
{
    public class Client
    {
        // Client  socket.
        private Socket socket;        
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();  

        public Client(Socket socket)
        {
            this.socket = socket;
        }

        public bool TryBeginOperation()
        {
            return false;
        }

        public void PushSendPacket(Packet packet)
        {

        }

        public void EndOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
