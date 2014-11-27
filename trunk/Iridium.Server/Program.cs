using System;

using Iridium.Server.Protocol;

namespace Iridium.Server
{
    class Program
    {
        private static NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();
        
        static void Main()
        {
            IridiumGameMasterServer masterServer = new IridiumGameMasterServer();
            IridiumMasterClientProtocol.Init(masterServer);

            masterServer.Start();

            Console.ReadKey();

            masterServer.Stop();
        }
    }
}
