using System;
using System.Configuration;

using Iridium.Server.Protocol;

using LinqToDB.Data;
using LinqToDB.DataProvider.MySql;
using IridiumDatabase;

namespace Iridium.Server
{
    public class Program
    {
        private static NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        private const string ConfigurationSectionName = "IridiumServer";
        private readonly IridiumConfig config;
        private string connectionString;

        public Program()
        {
            ConfigurationManager.RefreshSection(ConfigurationSectionName);
            this.config = ConfigurationManager.GetSection(ConfigurationSectionName) as IridiumConfig;
            IridiumConfig.Database pushDatabase = this.config.MysqlIridium;

            this.connectionString = string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};",
                                                 pushDatabase.Host,
                                                 pushDatabase.Port,
                                                 pushDatabase.Schema,
                                                 pushDatabase.User,
                                                 pushDatabase.Password);

            DataConnection.AddConfiguration(this.connectionString, this.connectionString, new MySqlDataProvider());

            using (var db = new iridiumDB(this.connectionString))
            { }

            IridiumGameMasterServer masterServer = new IridiumGameMasterServer();
            IridiumMasterClientProtocol.Init(masterServer);

            masterServer.Start();

            Console.ReadKey();

            masterServer.Stop();

            Console.WriteLine("Server already stoped! Press any key to close.");
            Console.ReadKey();
        }

        static void Main()
        {
           
            var p = new Program();
        }
    }
}
