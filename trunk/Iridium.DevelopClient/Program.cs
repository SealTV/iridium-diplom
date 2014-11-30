using System;
using System.Threading;
using Iridium.Utils.Data;

namespace Iridium.DevelopClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread[] threads =  new Thread[10];

            Random random = new Random();
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {

                    Thread.Sleep(random.Next(100, 150));
                    Client client = new Client(27001, "127.0.0.1");
                    client.Connect();
                    client.SendPacket(new Ping(10));

                    Thread.Sleep(1000);

                    client.Disconnect();
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Console.ReadKey();
        }
    }
}
