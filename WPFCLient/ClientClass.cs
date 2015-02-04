namespace Iridium.WPFClient
{
    using Iridium.Network;

    public class ClientClass
    {
        private static ClientClass client;
        public static ClientClass Client
        {
            get { return client ?? (client = new ClientClass()); }
        }


        private NetworkClient netClient;

        private ClientClass()
        {
            //this.netClient = new NetworkClient(27001, );
        }
    }
}
