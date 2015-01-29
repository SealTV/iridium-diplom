

namespace Iridium.Server
{
    using System;
    using System.Configuration;

    using Iridium.Server.PacketHandlers.FromClient;
    using Iridium.Server.Protocol;
    using Iridium.Utils.Data;

    using LinqToDB.Data;
    using LinqToDB.DataProvider.MySql;

    public class Program
    {
        public static IridiumConfig Configuration;
        public static string ConnectionString;

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private const string ConfigurationSectionName = "IridiumServer";
        private IridiumGameMasterServer masterServer;
       
        public Program()
        {
            ConfigurationManager.RefreshSection(ConfigurationSectionName);
            Program.Configuration = ConfigurationManager.GetSection(ConfigurationSectionName) as IridiumConfig;
            IridiumConfig.Database pushDatabase = Program.Configuration.MysqlIridium;

            Program.ConnectionString = string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};",
                                                 pushDatabase.Host,
                                                 pushDatabase.Port,
                                                 pushDatabase.Schema,
                                                 pushDatabase.User,
                                                 pushDatabase.Password);
            Console.WriteLine(ConnectionString);
            DataConnection.AddConfiguration(Program.ConnectionString, Program.ConnectionString, new MySqlDataProvider());

            masterServer = new IridiumGameMasterServer();

        }
        
        private void Start()
        {
            IridiumMasterClientProtocol.Init(masterServer);
            masterServer.Start();
        }

        private void Stop()
        {
            masterServer.Stop();
        }

        private static void Main()
        {
            var p = new Program();
            p.Start();
            Console.ReadKey();
            Console.WriteLine("Server already stoped! Press any key to close.");
            p.Stop();
        }
    }
}
