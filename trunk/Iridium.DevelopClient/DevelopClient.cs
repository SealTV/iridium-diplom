namespace Iridium.DevelopClient
{
    using System;
    using Iridium.Utils.Data;
    
    using Utils;

    internal class DevelopClient
    {
        private static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                NetworkClient networkClient = new NetworkClient(27001, "127.0.0.1");
                //NetworkClient networkClient = new NetworkClient(27001, "176.103.146.173");
                networkClient.Connect();
                Console.WriteLine("Connected!");
                networkClient.SendPacket(new PacketsFromClient.Ping(10));
                networkClient.SendPacket(new PacketsFromClient.Ping(20));
                networkClient.SendPacket(new PacketsFromClient.GetGames());
//                while (networkClient.Connected)
//                {
//                    var packets = networkClient.ReadAllPackets();
//                    foreach (var packet in packets)
//                    {
//                        Console.WriteLine(packet.PacketType);
//                    }
//                }
                Console.WriteLine();
                networkClient.Disconnect();

            }
            Console.ReadKey();
        }

    }
}
