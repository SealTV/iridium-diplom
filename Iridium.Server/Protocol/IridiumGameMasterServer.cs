using System;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Iridium.Utils.Data;

namespace Iridium.Server.Protocol
{
    public class IridiumGameMasterServer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private bool isWorking;
        private readonly Socket listener;
        private readonly IPEndPoint ipEndPoint;
        //private Task acceptNewClientsTask;
        private Task clientsManagerTask;

        private readonly ConcurrentQueue<Client> clients; 

        public IridiumGameMasterServer()
        {
            this.clients = new ConcurrentQueue<Client>();
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new ConcurrentQueue<Client>();
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

            var client = new Client(clientSocket);

            HandleNewClient(client);

            Logger.Trace("End accept new tcp Client.");

        }

        private void HandleNewClient(Client client)
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

                Client client;
                Logger.Info("{0} Connected!");
                if (!clients.TryDequeue(out client)) 
                    continue;
                Packet receivedPacket;
                if (client.TryGetPacket(out receivedPacket))
                {
                    IridiumMasterClientProtocol.ClientProtocolHandler.HandleNextClient(client, receivedPacket);
                }
                else
                {
                    this.clients.Enqueue(client);
                }

            }
        }

        public void AddClient(Client client)
        {
            this.clients.Enqueue(client);
        }

        public void Disconnect(Client client)
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
