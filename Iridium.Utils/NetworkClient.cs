namespace Iridium.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
    using Utils.Data;

    public partial class NetworkClient
    {
        private readonly IPEndPoint ipEndPoint;
        private Socket socket;

        private NetworkStream inputStream;
        private NetworkStream outputStream;

        public Guid SessionId { get; set; }

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
            this.socket.Connect(this.ipEndPoint);
            if (this.socket.Connected)
            {
                this.inputStream = new NetworkStream(this.socket, FileAccess.Read);
                this.outputStream = new NetworkStream(this.socket, FileAccess.Write);
            }
        }

        public void SendPacket(Packet packet)
        {
            if(packet == null)
                return;
            try
            {
                using (var stream = new MemoryStream())
                using (var writer = new BinaryWriter(stream))
                {
                    using (var dataStream = new MemoryStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(dataStream, packet);
                        int packetSize = (int) dataStream.Length;

                        writer.Write(packetSize);
                        writer.Write(dataStream.GetBuffer(), 0, packetSize);

                        this.outputStream.Write(stream.GetBuffer(), 0, (int) stream.Length);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SendPackets(IEnumerable<Packet> packets)
        {
            if(packets == null)
                return;
            foreach (var packet in packets)
            {
                this.SendPacket(packet);
            }     
        }

        public bool TryReadNextPacket(out Packet packet)
        {
            if (this.inputStream.DataAvailable)
            {
                using (var reader = new BinaryReader(this.inputStream))
                {
                    int packetSize = reader.ReadInt32();
                    byte[] data = new byte[1024];
                    int bytesReaded = reader.Read(data, 0, packetSize);
                    if (bytesReaded != packetSize)
                    {
                        packet = null;
                        return false;
                    }
                    using (var stream = new MemoryStream(data))
                    {
                        var formatter = new BinaryFormatter();
                        packet = formatter.Deserialize(stream) as Packet;
                        return true;
                    }
                }
            }
            packet = null;
            return false;
        }

        public Packet ReadNextPacket()
        {
            Packet resultPacket;
            this.TryReadNextPacket(out resultPacket);
            return resultPacket;
        }

        public IEnumerable<Packet> ReadAllPackets()
        {
            List<Packet> packets = new List<Packet>();
            Packet packet;
            while (this.TryReadNextPacket(out packet))
            {
                packets.Add(packet);
            }
            return packets;
        }

        public void Disconnect()
        {
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
        }
    }
}
