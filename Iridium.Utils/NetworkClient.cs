namespace Iridium.Utils
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    public partial class NetworkClient
    {
        private readonly IPEndPoint ipEndPoint;
        private Socket socket;

        private NetworkStream inputStream;
        private NetworkStream outputStream;

        public Guid SessionId { get; set; }
        public bool Connected { get { return socket.Connected; } }


        public NetworkClient(int port, string ipAddress)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        public NetworkClient(Socket clientSocket)
        {
            if (clientSocket == null)
                throw new NullReferenceException();
            this.ipEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
            this.socket = clientSocket;
            this.inputStream = new NetworkStream(this.socket, FileAccess.Read);
            this.outputStream = new NetworkStream(this.socket, FileAccess.Write);
        }


        public void Connect()
        {
            if (socket != null && socket.Connected)
                return;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                this.socket.Connect(this.ipEndPoint);
                if (this.socket.Connected)
                {
                    this.inputStream = new NetworkStream(this.socket, FileAccess.Read);
                    this.outputStream = new NetworkStream(this.socket, FileAccess.Write);
                }
            }
            catch (Exception e)
            {
                this.Connect();
            }
        }


        public bool SocketConnected()
        {
            bool part1 = this.socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (this.socket.Available == 0);
            return !part1 || !part2;
        }

        public void Disconnect()
        {
            try
            {
                //this.socket.Shutdown(SocketShutdown.Both);
                this.socket.Close();
                socket.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
