namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Text;
    using System.Threading;
    using Iridium.Network;
    using Iridium.Client.PacketHandlers;
    using Iridium.Utils.Data;
    using UnityEngine;

    public delegate void GamesLoaded(SharedData.GameData[] games);
    public delegate void LevelsLoaded(PacketsFromMaster.GameData gameData);
    public delegate void LevelDataLoaded(PacketsFromMaster.LevelData levelData);
    public delegate void Connected();
    public delegate void LoggedOnServer();

    public interface IServerConnector
    {
        void Init();
        void Connect(int port, string ip);
        
        void GetGames();
        void GetLevels(int gameId);
        void GetLevelData(int gameId, int levelId);

        event GamesLoaded OnGamesLoaded;
        event LevelsLoaded OnLevelsLoaded;
        event LevelDataLoaded OnLevelDataLoaded;
        event Connected OnConnectedToServer;
        event LoggedOnServer OnLoggedOnServer;
    }
    public class ServerConnector:MonoBehaviour,IServerConnector
    {
        private const int timeOut = 10000;
        private SharedData.GameData[] games;
        private string[] levels;

        private NetworkClient client;
        private string serverIpAddress;
        private int serverPort;

        public void Init()
        {
        }

        public void Connect(int port, string ip)
        {
            this.serverPort = port;
            this.serverIpAddress = ip;
            this.StartCoroutine(this.StartConnectServer());

        }

        public void Login(string login, string password)
        {
            this.StartCoroutine(this.StartLogin(login, password));
        }

        public void GetGames()
        {         
            this.StartCoroutine(this.GetGameListFromServer());
        }

        public void GetLevels(int gameId)
        {
            
        }

        public void GetLevelData(int gameId, int levelId)
        {
            throw new NotImplementedException();
        }

        private IEnumerator StartConnectServer()
        {
            try
            {
                this.client = new NetworkClient(this.serverPort, this.serverIpAddress);
                this.client.Connect();
                Packet pak = this.client.WaitNextPacket(timeOut);
                Debug.Log(pak.PacketType);
                this.OnConnectedToServer();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            yield break;
        }

        private IEnumerator StartLogin(string login, string password)
        {
            try
            {
                this.client.SendPacket(new PacketsFromClient.Login(login, Encoding.UTF8.GetBytes(password)));
                Packet pak = this.client.WaitNextPacket(timeOut) as PacketsFromMaster.LoginResult;
                Debug.Log(pak.PacketType);
                this.OnLoggedOnServer();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            yield break;
        }

        private IEnumerator Send()
        {
            this.client.SendPacket(new PacketsFromClient.GameAlgorithm(1, 1, "Console.WriteLine(124);"));
            var pak = this.client.WaitNextPacket(timeOut) as PacketsFromMaster.AlgorithmResult;
            foreach (var s in pak.Output)
            {
                Debug.Log(s);
            }
            Debug.Log(pak.IsSuccess);
            yield break;
        }
        private IEnumerator GetGameListFromServer()
        {
            this.client.SendPacket(new PacketsFromClient.GetGames());
            var pak = this.client.WaitNextPacket(timeOut) as PacketsFromMaster.GamesList;
            Debug.Log(pak.PacketType);
            this.games = pak.Games;
            this.OnGamesLoaded(this.games);
            yield break;
        }

        public event GamesLoaded OnGamesLoaded;
        public event LevelsLoaded OnLevelsLoaded;
        public event LevelDataLoaded OnLevelDataLoaded;
        public event Connected OnConnectedToServer;
        public event LoggedOnServer OnLoggedOnServer;

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
