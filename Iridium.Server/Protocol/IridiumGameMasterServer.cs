namespace Iridium.Server.Protocol
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;

    using Iridium.Network;

    using Utils.Data;

    public class IridiumGameMasterServer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private bool isWorking;
        private readonly Socket listener;
        private readonly IPEndPoint ipEndPoint;
        private Task clientsManagerTask;

        private readonly ConcurrentQueue<NetworkClient> clients; 

        public IridiumGameMasterServer()
        {
            this.clients = new ConcurrentQueue<NetworkClient>();
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(IridiumMasterServer.Configuration.ServerProperties.Host), IridiumMasterServer.Configuration.ServerProperties.Port);
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new ConcurrentQueue<NetworkClient>();
        }

        public void Start()
        {
            this.isWorking = true;
            Logger.Info("Sever started. Wait new connections.");

            this.listener.Bind(this.ipEndPoint);
            this.listener.Listen(200);

            BeginAcceptNewClient();
            this.clientsManagerTask = Task.Factory.StartNew(this.ManagedClientsPackets);

            //var clientsManager = Utils.PeriodicTaskFactory.Start(this.ManagedClientsPackets, 100, 0, -1, -1, true, new CancellationToken(), TaskCreationOptions.LongRunning);
        }

        private void BeginAcceptNewClient()
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
                Logger.Info("Client disconnected!");
            }

            if(clientSocket == null)
                return;

            BeginAcceptNewClient();

            var client = new NetworkClient(clientSocket);

            HandleNewClient(client);

            Logger.Trace("End accept new tcp Client.");

        }

        private void HandleNewClient(NetworkClient client)
        {
            client.SendPacket(new PacketsFromMaster.ServerInfo(client.SessionId));
            this.clients.Enqueue(client);
        }

        private void ManagedClientsPackets()
        {
            while (this.isWorking)
            {
                if (clients.Count > 0)
                {
//                    Logger.Debug("Clients connected = {0}", clients.Count);
                    NetworkClient client;
                    if (!clients.TryDequeue(out client))
                        continue;
                    IridiumMasterClientProtocol.ClientProtocolHandler.HandleNextClientAsync(client);
                }
                else
                {
                    Thread.Sleep(100);                    
                }
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
