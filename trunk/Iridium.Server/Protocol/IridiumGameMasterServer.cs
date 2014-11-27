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
        // Thread signal.
        private static ManualResetEvent allDone;

        public IridiumGameMasterServer()
        {
            this.clients = new ConcurrentQueue<Client>();
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            allDone = new ManualResetEvent(false);
            clients = new ConcurrentQueue<Client>();
        }

        public void Start()
        {
            this.isWorking = true;
            Logger.Trace("Sever started. Wait new connections.");

            //this.acceptNewClientsTask = Task.Factory.StartNew(this.WaitClient);
            BeginAcceptNewclient();
            this.clientsManagerTask = Task.Factory.StartNew(this.ManagedClientsPackets);
        }

        public void Stop()
        {
            Logger.Trace("Sever stoped.");
            this.isWorking = false;
            this.listener.Shutdown(SocketShutdown.Both);

            this.listener.Close(10);

            //Logger.Trace(this.acceptNewClientsTask.Status);
            //try
            //{
            //    this.acceptNewClientsTask.Wait(new TimeSpan(0, 0, 10));
            //}
            //catch (Exception e)
            //{
            //    Logger.Warn(e);
            //}
            Logger.Trace(this.clientsManagerTask.Status);
            try
            {
                this.clientsManagerTask.Wait(new TimeSpan(0, 0, 10));
            }
            catch (Exception e)
            {
                Logger.Warn(e);
            }
        }

        private void BeginAcceptNewclient()
        {
            if(!this.isWorking)
                return;
            this.listener.BeginAccept(AcceptNewClientCallback, this.listener);
        }

        private void AcceptNewClientCallback(IAsyncResult ar)
        {
            Logger.Trace("Accept new tcp Client.");
            var socket = (Socket) ar.AsyncState;
            var clientSocket = socket.EndAccept(ar);
            BeginAcceptNewclient();

            var client = new Client(clientSocket);
            this.clients.Enqueue(client);
            Logger.Trace("End accept new tcp Client.");
        }

        // TODO test async callback.... remove next two methods.
        #region Methods for cleaning
        private void WaitClient()
        {
            try
            {
                this.listener.Bind(this.ipEndPoint);
                this.listener.Listen(200);

                while (this.isWorking)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();
                    this.listener.BeginAccept(AcceptCallback, this.listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
                this.listener.Close(100);
            }
            catch (SocketException e)
            {
                Logger.Error("{0}", e);
            }
        }
        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();
            
            Logger.Trace("Accept new tcp Client.");
            var socket = ar.AsyncState as Socket;

            if (socket == null) 
                return;
            var client = new Client(socket.EndAccept(ar));
            this.clients.Enqueue(client);
            Logger.Trace("End accept new tcp Client.");
        }
        #endregion

        private void ManagedClientsPackets()
        {
            while (this.isWorking)
            {
                Client client;
                if (!clients.TryDequeue(out client)) 
                    continue;
                Packet receivedPacket;
                if (client.TryGetPacket(out receivedPacket))
                {
                    IridiumMasterClientProtocol.ClientProtocolHandler.HandleNextClient(client);
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
    }
}
