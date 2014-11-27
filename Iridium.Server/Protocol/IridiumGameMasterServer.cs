using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Iridium.Utils.Data;

namespace Iridium.Server
{
    public class IridiumGameMasterServer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private bool isWorking;
        private readonly Socket listener;
        private readonly IPEndPoint ipEndPoint;
        private Task waitClientTask;
        private Task clientWorkerTask;

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

            this.clientWorkerTask = Task.Factory.StartNew(this.ClientWorker);
            this.waitClientTask   = Task.Factory.StartNew(this.WaitClient);
        }

        public void Stop()
        {
            Logger.Trace("Sever stoped.");
            this.isWorking = false;
            Logger.Trace(this.waitClientTask.Status);
        }

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
                    this.listener.BeginAccept(new AsyncCallback(AcceptCallback), this.listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
                this.listener.Close(100);
            }
            catch (SocketException exc)
            {
                Logger.Error("{0}", exc);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();
            
            Logger.Trace("Accept new tcp Client.");
            var socket = ar.AsyncState as Socket;

            if (socket != null)
            {
                var client = new Client(socket.EndAccept(ar));
                this.clients.Enqueue(client);
                Logger.Trace("End accept new tcp Client.");
            }
        }

        private void ClientWorker()
        {
            while (this.isWorking)
            {
                Client workClient;
                Packet workPacket;
                if (clients.TryDequeue(out workClient))
                {
                    if (workClient.TryGetPacket(out workPacket))
                    {
                        
                    }
                    else
                    {
                        this.AddClient(workClient);
                    }
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
