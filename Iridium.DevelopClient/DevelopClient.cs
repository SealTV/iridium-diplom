namespace Iridium.DevelopClient
{
    using System;
    using System.Text;
    using System.Threading;

    using Iridium.Network;
    using Iridium.Utils.Data;

    internal class DevelopClient
    {
        private static void Main(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
               Run();
            }
            Console.ReadKey();
        }

        private static async void Run()
        {
            NetworkClient networkClient = new NetworkClient(27001, "104.40.216.136");
            networkClient.Connect();
            Console.WriteLine("Connected!");
            Thread.Sleep(100);
           
            var packet = await networkClient.ReadNextPacket();

            if (packet != null)
                Console.WriteLine(packet.PacketType);

//            networkClient.SendPacket(new PacketsFromClient.Register("seal", Encoding.UTF8.GetBytes("seal123")));
//            Thread.Sleep(100);
//
//            packet = await networkClient.ReadNextPacket();
//
//            if (packet != null)
//                Console.WriteLine(packet.PacketType);

            //            const string code = "Console.WriteLine(container.Enemies.Length);" +
            //                                "return 0;";

            networkClient.SendPacket(new PacketsFromClient.Login("seal", Encoding.UTF8.GetBytes("seal123")));
            Thread.Sleep(100);
            packet = await networkClient.ReadNextPacket();
            if (packet != null)
                Console.WriteLine(packet.PacketType);


            
            networkClient.SendPacket(new PacketsFromClient.Ping(10));

            Thread.Sleep(100);
            packet = await networkClient.ReadNextPacket();
            if (packet != null)
                Console.WriteLine(packet.PacketType);

            networkClient.SendPacket(new PacketsFromClient.Ping(20));
            Thread.Sleep(100);
            packet = await networkClient.ReadNextPacket();
            if (packet != null)
                Console.WriteLine(packet.PacketType);

            networkClient.SendPacket(new PacketsFromClient.GetGames());
            Thread.Sleep(100);
            packet = await networkClient.ReadNextPacket();
            if (packet != null)
                Console.WriteLine(packet.PacketType);

            Console.WriteLine();
            Console.ReadLine();
            networkClient.Disconnect();
        }

    }
}
