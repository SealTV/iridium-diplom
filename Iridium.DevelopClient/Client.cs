using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

using Iridium.Utils.Data;

namespace Iridium.DevelopClient
{
    public partial class Client
    {
        private readonly TcpClient client;
        private readonly IPEndPoint ipEndPoint;
        private Socket socket;


        private NetworkStream inputStream;
        private NetworkStream outputStream;

        private BinaryReader BinaryReader;
        private BinaryWriter BinaryWriter;

        private BinaryFormatter formatter;

        public Client(int port, string ipAddress)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            client = new TcpClient();
            this.formatter = new BinaryFormatter();
        }

        public void Connect()
        {
            this.client.Connect(ipEndPoint);
            this.socket = this.client.Client;

            this.inputStream = new NetworkStream(this.socket, FileAccess.Read);
            this.outputStream = new NetworkStream(this.socket, FileAccess.Write);

            this.BinaryReader = new BinaryReader(this.inputStream);
            this.BinaryWriter = new BinaryWriter(this.outputStream);
        }

        public void SendPacket(Packet packet)
        {
            if(packet == null)
                return;
            this.formatter.Serialize(this.inputStream, packet);
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
                packet = this.formatter.Deserialize(this.inputStream) as Packet;
                return true;
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
                packet = null;
            }
            return packets;
        }  

        public void Close()
        {
            this.client.Close();
        }
    }
}
