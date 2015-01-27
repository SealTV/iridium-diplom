namespace Iridium.Network
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Runtime.Serialization.Formatters.Binary;
    using Iridium.Utils.Data;

    /// <summary>
    ///In this class write async methods for send and receive packets.
    /// </summary>
    public partial class NetworkClient
    {
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

                            this.outputStream.WriteAsync(stream.GetBuffer(), 0, (int)stream.Length);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
        }

        public async Task<Packet> ReadNextPacket()
        {
            if (!this.inputStream.DataAvailable)
                return null;
            var reader = new BinaryReader(this.inputStream);
            {
                int packetSize = reader.ReadInt32();
                var stream = new MemoryStream(reader.ReadBytes(packetSize));
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as Packet;
            }
        }
    }
}
