using System;

using Iridium.Server.Protocol;

namespace Iridium.Server
{
    public static class Program
    {
        private static NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();
        
        static void Main()
        {
            //using (var db = new iridium_master_serverDB())
            //{}
            
            IridiumGameMasterServer masterServer = new IridiumGameMasterServer();
            IridiumMasterClientProtocol.Init(masterServer);

            masterServer.Start();

            Console.ReadKey();

            masterServer.Stop();

            Console.WriteLine("Server already stoped! Press any key to close.");
            Console.ReadKey();

        }
    }
}
