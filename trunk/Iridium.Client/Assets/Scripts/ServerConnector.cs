using System.Net;
using System.Net.Sockets;

namespace Assets
{
    using System.IO;
    using System.Threading;
    using UnityEngine;

    public class ServerConnector : MonoBehaviour
    {
        private bool isConnect;
        private Stream workStream;
        private Thread thread;
        private Socket socket;

        private IPAddress serverIpAddress;
        private int serverPort;

        private void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Connect();
        }

        private void Init()
        {
            serverIpAddress = IPAddress.Parse("127.0.0.1");
            serverPort = 27001;
        }

        private void Connect()
        {
            thread = new Thread(new ThreadStart(this.ConnectAsync));
            thread.Start();
        }

        private void Update()
        {
            Debug.Log(isConnect);
        }

        private void ConnectAsync()
        {
            socket.Connect(serverIpAddress, serverPort);
            isConnect = true;
        }

        private void Reconnect()
        {
            
        }
    }
}
