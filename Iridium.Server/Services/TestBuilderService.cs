namespace Iridium.Server.Services
{
    using System.Collections.Concurrent;
    using Protocol;
    using Utils;

    public class TestBuilderService
    {
        private const string MSBUILDER_PATH = "";

        private IridiumGameMasterServer iridiumGameMasterServer;
        private System.Collections.Concurrent.ConcurrentQueue<NetworkClient> networkClients;
       
        private static TestBuilderService _testBuilder;

        private static TestBuilderService TestBuilder
        {
            get { return _testBuilder; }
            set { _testBuilder = value; }
        }

        private TestBuilderService(IridiumGameMasterServer iridiumGameMasterServer)
        {
            this.iridiumGameMasterServer = iridiumGameMasterServer;
            this.networkClients = new ConcurrentQueue<NetworkClient>();
        }

        public static void Init(IridiumGameMasterServer masterServer)
        {
            TestBuilder = new TestBuilderService(masterServer);
        }


        public static void SetNewClientInQueue(NetworkClient client)
        {
            TestBuilder.networkClients.Enqueue(client);
        }
    }
}
