using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

using Iridium.Utils.Data;

namespace Iridium.DevelopClient
{
    /// <summary>
    ///In this class write async methods for send and receive packets.
    /// </summary>
    public partial class Client
    {
        public async void SendPacketAsync(Packet packet)
        {
            var k = this.client.ReceiveBufferSize;

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, packet);

                await this.outputStream.WriteAsync(stream.GetBuffer(), 0, (int) stream.Length);
            }
        }

        public async Task<object> ReadNextPacketAsync()
        {
            Packet packet = null;
            if (!this.inputStream.DataAvailable)
                return null;

            byte[] buffer = new byte[1024];
            Task<int> readedData = this.inputStream.ReadAsync(buffer, 0, (int)this.inputStream.Length);

            using (var stream = new MemoryStream(buffer))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, packet);
            }
            return null;
        }

    //using (var networkStream = client.GetStream()) //get access to stream
    //        {
    //            while (networkStream.DataAvailable) //still has some data
    //            {
    //                var buffer = new byte[1024]; //get a buffer
    //                int byteReaded = await networkStream.ReadAsync(buffer, 0, buffer.Length); //read from network there

    //                //om nom nom buffer     
    //                using (var ms = new MemoryStream()) //process just one chunk
    //                {
    //                    ms.Write(buffer, 0, byteReaded);
    //                    var formatter = new BinaryFormatter();
    //                    packet = formatter.Deserialize(ms) as Packet;   //desserialise the object        
    //                } // dispose memory
    //                //async send obj up for further processing
    //            }
    //        }
    //        return packet;
    //    }
    }
}
