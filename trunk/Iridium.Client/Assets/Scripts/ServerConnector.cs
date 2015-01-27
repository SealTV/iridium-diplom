namespace Assets.Scripts
{
    using Iridium.Utils.Data;
    using UnityEngine;

    public delegate void GamesLoaded(GameData[] games);
    public delegate void LevelsLoaded(LevelData[] levels);

    public interface IServerConnector
    {
        void Init();
        void Connect(int port, string ip);
        
        void GetGames();
        void GetLevels(int gameId);

        bool TryGetLevels(ref LevelData[] gameDatas);
        event GamesLoaded OnGamesLoaded;
        event LevelsLoaded OnLevelsLoaded;
    }
    public class ServerConnector:MonoBehaviour,IServerConnector
    {
//        private bool isConnect;
//        private Stream workStream;
//        private Thread thread;
//        private Socket socket;

//        private IPAddress serverIpAddress;
//        private int serverPort;

//        private void Start()
//        {
//            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            this.Connect();
//        }

//        private void Init()
//        {
//            this.serverIpAddress = IPAddress.Parse("127.0.0.1");
//            this.serverPort = 27001;
//        }

//        private void Connect()
//        {
//            this.thread = new Thread(new ThreadStart(this.ConnectAsync));
//            this.thread.Start();
//        }

//        private void Update()
//        {
//            Debug.Log(this.isConnect);
//        }

//        private void ConnectAsync()
//        {
//            this.socket.Connect(this.serverIpAddress, this.serverPort);
//            this.isConnect = true;
//        }

//        private void Reconnect()
//        {
            
//        }
        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void Connect(int port, string ip)
        {
            throw new System.NotImplementedException();
        }

        public void GetGames()
        {
            throw new System.NotImplementedException();
        }

        public void GetLevels(int gameId)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetLevels(ref LevelData[] gameDatas)
        {
            throw new System.NotImplementedException();
        }

        public event GamesLoaded OnGamesLoaded;
        public event LevelsLoaded OnLevelsLoaded;
    }

    
}
