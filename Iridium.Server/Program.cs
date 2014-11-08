using System;

namespace Iridium.Server
{
    class Program
    {
        private static NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();
        
        static void Main(string[] args)
        {
            IridiumGameMasterServer masterServer = new IridiumGameMasterServer();
            masterServer.Start();

            Console.ReadKey();

            masterServer.Stop();
        }
    }
}
