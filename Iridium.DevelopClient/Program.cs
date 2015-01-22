using System;
using System.Threading;
using Iridium.Utils.Data;

namespace Iridium.DevelopClient
{
    using Utils;

    class Program
    {
        private static void Main(string[] args)
        {
            Thread[] threads = new Thread[1];

            Random random = new Random();
            for (int i = 0; i < threads.Length; i++)
            {
                //threads[i] = new Thread(() =>
                //{

                //Thread.Sleep(random.Next(100, 150));
                NetworkClient networkClient = new NetworkClient(27001, "127.0.0.1");
                networkClient.Connect();
                Console.WriteLine("Connected!");
                var packets = networkClient.ReadAllPackets();

                foreach (var packet in packets)
                {
                    Console.WriteLine(packet.PacketType);
                }

                networkClient.SendPacket(new Ping(10));
                Console.WriteLine("Sent packet!");
                //Thread.Sleep(1000);
                networkClient.Disconnect();
                //});
            }

            //foreach (var thread in threads)
            //{
            //    thread.Start();
            //}

            Console.ReadKey();
        }
    }
}
