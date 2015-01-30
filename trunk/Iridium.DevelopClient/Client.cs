namespace Iridium.DevelopClient
{
    using System.IO;
    using System.Net.Sockets;

    public class Client
    {
        public Socket Socket { get; set; }

        private NetworkStream input;
        public NetworkStream InputStream
        {
            get { return this.input ?? (this.input = new NetworkStream(this.Socket, FileAccess.Read)); }
            private set { this.input = value; }
        }

        private NetworkStream output;
        public NetworkStream OutputStream
        {
            get { return this.output ?? (this.output = new NetworkStream(this.Socket, FileAccess.Write)); }
            private set { this.output = value; }
        }

        public void ReadPacket()
        {
            using (var stream = new NetworkStream(Socket, FileAccess.Read))
            {
                
            }
        }
    }
}
