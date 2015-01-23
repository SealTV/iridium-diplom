using System;
using Iridium.Utils.Data;

namespace Iridium.DevelopClient
{
    using Utils;

    internal class DevelopClient
    {
        private static void Main(string[] args)
        {
            NetworkClient networkClient = new NetworkClient(27001, "127.0.0.1");
            networkClient.Connect();
            Console.WriteLine("Connected!");
            networkClient.SendPacket(new Ping(10));
            networkClient.SendPacket(new Ping(20));
            networkClient.SendPacket(new GetGamesPacket());
            while (networkClient.Connected)
            {
                var packets = networkClient.ReadAllPackets();
                foreach (var packet in packets)
                {
                    Console.WriteLine(packet.PacketType);
                }
            }
            Console.WriteLine();
            networkClient.Disconnect();

            Console.ReadKey();
        }

    }
}
