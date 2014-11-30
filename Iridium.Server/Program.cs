using System;

using Iridium.Server.Protocol;
using IridiumDatabase;

namespace Iridium.Server
{
    class Program
    {
        private static NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();
        
        static void Main()
        {


            using (var db = new iridium_master_serverDB())
            {
                
            }
            
            IridiumGameMasterServer masterServer = new IridiumGameMasterServer();
            IridiumMasterClientProtocol.Init(masterServer);

            masterServer.Start();

            Console.ReadKey();

            masterServer.Stop();
        }
    }
}
