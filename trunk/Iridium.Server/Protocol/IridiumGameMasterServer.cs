namespace Iridium.Server.Protocol
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;

    using Utils;
    using Utils.Data;

    public class IridiumGameMasterServer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private bool isWorking;
        private readonly Socket listener;
        private readonly IPEndPoint ipEndPoint;
        //private Task acceptNewClientsTask;
        private Task clientsManagerTask;

        private readonly ConcurrentQueue<NetworkClient> clients; 

        public IridiumGameMasterServer()
        {
            this.clients = new ConcurrentQueue<NetworkClient>();
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new ConcurrentQueue<NetworkClient>();
        }

        public void Start()
        {
            this.isWorking = true;
            Logger.Trace("Sever started. Wait new connections.");

            this.listener.Bind(this.ipEndPoint);
            this.listener.Listen(200);

            BeginAcceptNewclient();
            this.clientsManagerTask = Task.Factory.StartNew(this.ManagedClientsPackets);
        }

        private void BeginAcceptNewclient()
        {
            if(!this.isWorking)
                return;
            this.listener.BeginAccept(EndAcceptCallback, this.listener);
        }

        private void EndAcceptCallback(IAsyncResult ar)
        {
            var socket = (Socket) ar.AsyncState;
            Socket clientSocket = null;
            try
            {
                clientSocket = socket.EndAccept(ar);
                Logger.Trace("Accept new tcp Client.");
            }
            catch (Exception e)
            {
                //Logger.Info(e);
            }

            if(clientSocket == null)
                return;

            BeginAcceptNewclient();

            var client = new NetworkClient(clientSocket);

            HandleNewClient(client);

            Logger.Trace("End accept new tcp Client.");

        }

        private void HandleNewClient(NetworkClient client)
        {
            client.SendPacket(new ServerInfo
            {
               ClientId = client.SessionId,
            });
            this.clients.Enqueue(client);
        }

        private void ManagedClientsPackets()
        {
            while (this.isWorking)
            {
                Thread.Sleep(1000);

                NetworkClient client;
                Logger.Info("{0} Connected!");
                if (!clients.TryDequeue(out client)) 
                    continue;
                IridiumMasterClientProtocol.ClientProtocolHandler.HandleNextClient(client);
            }
        }

        public void AddClient(NetworkClient client)
        {
            this.clients.Enqueue(client);
        }

        public void Disconnect(NetworkClient client)
        {
            client.Disconnect();
        }

        public void Stop()
        {
            Logger.Trace("Stat stoping server.");
            this.isWorking = false;
            try
            {
                this.listener.Close(10);
            }
            catch (SocketException e)
            {
                Logger.Warn(e);
            }
            Logger.Trace(this.clientsManagerTask.Status);
            try
            {
                this.clientsManagerTask.Wait(new TimeSpan(0, 0, 10));
            }
            catch (Exception e)
            {
                Logger.Warn(e);
            }
            Logger.Trace("Sever stoped.");
        }

    }
}
