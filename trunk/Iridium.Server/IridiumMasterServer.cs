namespace Iridium.Server
{
    using System;
    using System.Configuration;
    using System.ServiceProcess;

    using Iridium.Server.Games;
    using Iridium.Server.PacketHandlers.FromClient;
    using Iridium.Server.Protocol;
    using Iridium.Utils.Data;

    using LinqToDB.Data;
    using LinqToDB.DataProvider.MySql;

    public class IridiumMasterServer : ServiceBase
    {
        public static IridiumConfig Configuration;
        public static string ConnectionString;

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static string ServiceApplicationName = "IridiumMasterService";
        private const string ConfigurationSectionName = "IridiumServer";
        private IridiumGameMasterServer masterServer;
       
        public IridiumMasterServer()
        {
            ConfigurationManager.RefreshSection(ConfigurationSectionName);
            IridiumMasterServer.Configuration = ConfigurationManager.GetSection(ConfigurationSectionName) as IridiumConfig;
            IridiumConfig.Database pushDatabase = IridiumMasterServer.Configuration.MysqlIridium;

            IridiumMasterServer.ConnectionString = string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};",
                                                 pushDatabase.Host,
                                                 pushDatabase.Port,
                                                 pushDatabase.Schema,
                                                 pushDatabase.User,
                                                 pushDatabase.Password);
            Console.WriteLine(ConnectionString);
            DataConnection.AddConfiguration(IridiumMasterServer.ConnectionString, IridiumMasterServer.ConnectionString, new MySqlDataProvider());

            masterServer = new IridiumGameMasterServer();

        }

        private static void Main(string[] args)
        {
           if (Environment.UserInteractive)
            {
                string option = args.Length > 0 ? args[0].ToUpperInvariant() : "";
                switch (option)
                {
                    case "/INSTALL":
                        IridiumServerInstaller.Install();
                        break;
                    case "/UNINSTALL":
                        IridiumServerInstaller.Uninstall();
                        break;
                    case "":
                        new IridiumMasterServer().RunConsole(args);
                        break;
                    default:
                        Console.WriteLine("Argument not recognized: {0}", option);
                        break;
                }
            }
            else
            {
                ServiceBase.Run(new IridiumMasterServer());
            }
        }

        private void RunConsole(string[] args)
        {
//            new GameAlgorithmPacketHandler(this.masterServer, new PacketsFromClient.GameAlgorithm(1, 1, "return Container.Enemies.Count;")).Run();
            //new Testing().Run();
            Console.SetWindowSize(150, 40);
            this.Start();
            Logger.Trace("Press any key to stop program");
            Console.Read();
            this.Stop();
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

        protected override void OnStart(string[] args)
        {
            this.Start();
        }

        protected override void OnStop()
        {
            this.Stop();
        }
    }
}
