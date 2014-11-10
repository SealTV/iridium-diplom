using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Iridium.Server
{
    public class IridiumGameMasterServer
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private bool isBegin;
        private Socket listener;
        private IPEndPoint ipEndPoint;
        private Task task;

        private BlockingCollection<Client> clients; 
        // Thread signal.
        public static ManualResetEvent allDone;

        public IridiumGameMasterServer()
        {
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            this.listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            allDone = new ManualResetEvent(false);
            clients = new BlockingCollection<Client>();
        }

        public void Start()
        {
            this.isBegin = true;
            Logger.Trace("Sever started. Wait new connections.");
            task = new Task(Begin);
            task.Start();
        }

        public void Stop()
        {
            Logger.Trace("Sever stoped.");
            this.isBegin = false;
            Logger.Trace(task.Status);
        }

        private void Begin()
        {
            try
            {
                this.listener.Bind(this.ipEndPoint);
                this.listener.Listen(200);

                while (this.isBegin)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();
                    this.listener.BeginAccept(AcceptCallback, this.listener);

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
                this.clients.Add(client);
                Logger.Trace("End accept new tcp Client.");
            }
        }

        private void Disconnect(Client client)
        {

        }
    }
}
