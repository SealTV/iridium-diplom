namespace Iridium.Utils
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Runtime.Serialization.Formatters.Binary;

    using Utils.Data;

    /// <summary>
    ///In this class write async methods for send and receive packets.
    /// </summary>
    public partial class NetworkClient
    {
        public async void SendPacketAsync(Packet packet)
        {
            if (packet == null)
                return;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, packet);
                int packetSize = (int)stream.Length;
                using (var memoryStream = new MemoryStream())
                using (var writer = new BinaryWriter(memoryStream))
                {
                    writer.Write(packetSize);
                    writer.Write(stream.GetBuffer(), 0, packetSize);
                    await this.outputStream.WriteAsync(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
        }

        public async Task<Packet> ReadNextPacketAsync()
        {
            Packet packet = null;
            if (!this.inputStream.DataAvailable)
                return null;
            byte[] buffer = new byte[1024];
            int readedData = await this.inputStream.ReadAsync(buffer, 0, buffer.Length);

            using (var memStream = new MemoryStream(buffer, 0, readedData))
            using (var reader = new BinaryReader(memStream))
            {
                int packetSize = reader.ReadInt32();

                var formatter = new BinaryFormatter();
                return formatter.Deserialize(memStream) as Packet;
            }
        }
    }
}
