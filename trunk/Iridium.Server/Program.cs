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
        public static IridiumConfig Configuration;
        public static string ConnectionString;

        private static NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

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

            DataConnection.AddConfiguration(Program.ConnectionString, Program.ConnectionString, new MySqlDataProvider());

            //using (var db = new iridiumDB(Program.ConnectionString))
            { }

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
            //Console.WriteLine("Write STOP!");
            //string str = Console.ReadLine();
            //while (str!="STOP")
            //{
            //    str = Console.ReadLine();
            //}
            Console.WriteLine("Server already stoped! Press any key to close.");

            p.Stop();
        }
    }
}
