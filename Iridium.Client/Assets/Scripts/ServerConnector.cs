//namespace Assets.Scripts
//{
//    using System.IO;
//    using System.Net;
//    using System.Net.Sockets;
//    using System.Threading;
//    using UnityEngine;

//    public class ServerConnector : MonoBehaviour
//    {
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
//    }
//}
