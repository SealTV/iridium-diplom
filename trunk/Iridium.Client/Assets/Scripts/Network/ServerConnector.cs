namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using Iridium.Network;
    using Iridium.Client.PacketHandlers;
    using Iridium.Utils.Data;
    using UnityEngine;

    public delegate void GamesLoaded(SharedData.GameData[] games);
    public delegate void LevelsLoaded(PacketsFromMaster.GameData gameData);
    public delegate void LevelDataLoaded();
    public delegate void Connected();
    public delegate void LoggedOnServer();
    public delegate void AlgorithmResultLoaded(PacketsFromMaster.AlgorithmResult result);

    public interface IServerConnector
    {
        void Init();
        IEnumerator StartConnectServer(int port, string ip);
        IEnumerator StartLogin(string login, string password);
        IEnumerator StartGetGames();
        IEnumerator StartGetLevels(int gameId);
        MainMenu.SocketStatus SocketStatus { get; set; }
        void GetLevels(int gameId);
        void GetLevelData(int gameId, int levelId);

        event GamesLoaded OnGamesLoaded;
        event LevelsLoaded OnLevelsLoaded;
        event LevelDataLoaded OnLevelDataLoaded;
        event Connected OnConnectedToServer;
        event LoggedOnServer OnLoggedOnServer;
        event AlgorithmResultLoaded OnAlgorithmResultLoaded;
    }
    public class ServerConnector:MonoBehaviour,IServerConnector
    {
        public static ServerConnector Instance;
        private int timeOut = 10000;
        private SharedData.GameData[] games;
        private PacketsFromMaster.GameData gameData;
        private PacketsFromMaster.AlgorithmResult algorithmResult;

        private NetworkClient client;
        private string serverIpAddress;
        private int serverPort;

        private Thread workThread;

        public void Awake()
        {
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        public void Init()
        {
        }

        public IEnumerator StartConnectServer(int port, string ip)
        {
            this.serverPort = port;
            this.serverIpAddress = ip;
            this.workThread = new Thread(this.ConnectServer);
            this.workThread.Start();
            while (this.workThread.IsAlive)
            {
                yield return null;
            }

            this.OnConnectedToServer();
        }
        public IEnumerator StartLogin(string login, string password)
        {
            this.workThread = new Thread(() => this.Login(login, password));
            this.workThread.Start();
            while (this.workThread.IsAlive)
            {
                yield return null;
            }
            this.OnLoggedOnServer();
        }
        public IEnumerator StartGetGames()
        {
            this.workThread = new Thread(this.GetGames);
            this.workThread.Start();
            while (this.workThread.IsAlive)
            {
                yield return null;
            }

            this.OnGamesLoaded(this.games);
        }
        public IEnumerator StartGetLevels(int gameId)
        {
            this.workThread = new Thread(() => this.GetLevels(gameId));
            this.workThread.Start();
            while (this.workThread.IsAlive)
            {
                yield return null;
            }
            this.OnLevelsLoaded(this.gameData);
        }

        public IEnumerator StartSendAlgoritm(int gameId, int levelId, string algoritm)
        {
            this.workThread = new Thread(() => this.SendAlgoritm(gameId, levelId, algoritm));
            Debug.Log("Sending algorithm");
            this.workThread.Start();
            while (this.workThread.IsAlive)
            {
                yield return null;
            }
            Debug.Log("Result responsed");
            OnAlgorithmResultLoaded(algorithmResult);
        }
        public MainMenu.SocketStatus SocketStatus { get; set; }

        public IEnumerator StartGetLevelData(int gameId, int levelId)
        {
            this.workThread = new Thread(() => this.GetLevelData(gameId, levelId));
            this.workThread.Start();
            while (this.workThread.IsAlive)
            {
                yield return null;
            }
            this.OnLevelDataLoaded();
        }
        

        public void ConnectServer()
        {
            try
            {
                this.client = new NetworkClient(this.serverPort, this.serverIpAddress);
                this.client.Connect();
                Packet pak = this.client.WaitNextPacket(this.timeOut);
                Debug.Log(pak.PacketType);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        private void Login(string login, string password)
        {
            try
            {
                this.client.SendPacket(new PacketsFromClient.Login(login, Encoding.UTF8.GetBytes(password)));
                Packet pak = this.client.WaitNextPacket(this.timeOut) as PacketsFromMaster.LoginResult;
                Debug.Log(pak.PacketType);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        private void GetGames()
        {

            this.client.SendPacket(new PacketsFromClient.GetGames());
            var pak = this.client.WaitNextPacket(this.timeOut) as PacketsFromMaster.GamesList;
            Debug.Log(pak.PacketType);
            this.games = pak.Games;
        }
        public void GetLevels(int gameId)
        {
            this.client.SendPacket(new PacketsFromClient.GetGameData(gameId));
            var pak = this.client.WaitNextPacket(this.timeOut) as PacketsFromMaster.GameData;
            this.gameData = pak;
        }

        public void GetLevelData(int gameId, int levelId)
        {
            this.client.SendPacket(new PacketsFromClient.GetLevelData(gameId, levelId));
            var pak = this.client.WaitNextPacket(this.timeOut) as PacketsFromMaster.LevelData;
            GlobalData.LevelData = pak;
        }

        public void SendAlgoritm(int gameId, int levelId, string algoritm)
        {
            this.client.SendPacket(new PacketsFromClient.GameAlgorithm(gameId, levelId, algoritm));
            Packet pak = this.client.WaitNextPacket(this.timeOut);
            Debug.Log(pak.PacketType);
            this.algorithmResult = pak as PacketsFromMaster.AlgorithmResult;
        }

        public event Connected OnConnectedToServer;
        public event LoggedOnServer OnLoggedOnServer;
        public event GamesLoaded OnGamesLoaded;
        public event LevelsLoaded OnLevelsLoaded;
        public event LevelDataLoaded OnLevelDataLoaded;
        public event AlgorithmResultLoaded OnAlgorithmResultLoaded;

        private PacketHandler GetPacketHandlerFor(Packet packet)
        {
            switch ((MasterServerPacketType)packet.PacketType)
            {
                    case MasterServerPacketType.GamesList: return new PacketHandler.GameListPacketHandler(packet,this);
                    case MasterServerPacketType.GameData:  return new PacketHandler.GameDataPacketHandler(packet,this);
                    default:                               throw  new Exception("Unknown packet type");
            }
        }
    }

    
}
