namespace Iridium.Network
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using Iridium.Utils.Data;

    public class NetworkClient
    {
        private readonly IPEndPoint ipEndPoint;
        private Socket socket;

        private NetworkStream inputStream;
        private NetworkStream outputStream;

        public Guid SessionId { get; set; }
        public uint AccountId { get; set; }
        public SessionState State { get; set; }

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
            this.State = SessionState.NotLogged;
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

        public void SendPacket(Packet packet)
        {
            if (packet == null)
                return;
            if (this.SocketConnected())
                try
                {
                    using (var stream = new MemoryStream())
                    using (var writer = new BinaryWriter(stream))
                    {
                        using (var dataStream = new MemoryStream())
                        {
                            var formatter = new BinaryFormatter();
                            formatter.Serialize(dataStream, packet);
                            int packetSize = (int)dataStream.Length;

                            writer.Write(packetSize);
                            writer.Write(dataStream.GetBuffer(), 0, packetSize);

                            this.outputStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
                this.socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Packet WaitNextPacket(int timeOut)
        {
            Stopwatch start = new Stopwatch();
            start.Start();
            Packet packet;
            while (!this.TryReadNextPacket(out packet))
            {
                if(start.ElapsedMilliseconds>timeOut) throw new Exception("Timeout exception");
                Thread.Sleep(10);
            }
            return packet;

        }

        public bool TryReadNextPacket(out Packet packet)
        {
            packet = null;
            if (!this.inputStream.DataAvailable)
                return false;
            var reader = new BinaryReader(this.inputStream);
            int packetSize = reader.ReadInt32();
            var stream = new MemoryStream(reader.ReadBytes(packetSize));
            var formatter = new BinaryFormatter();
            packet = formatter.Deserialize(stream) as Packet;
            return true;
        }
    }
}