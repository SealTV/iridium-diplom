using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Iridium.Server.Protocol;

using Iridiun.Utils.Data;

using NLog;

namespace Iridium.Server
{
    public class IridiumMasterRouter
    {
        /// <summary>
        /// Default concurrency level (taken from ConcurrentDictionary)
        /// </summary>
        private static readonly int DefaultConcurrencyLevel = 4 * Environment.ProcessorCount;
        
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly IridiumMasterRouter instance = new IridiumMasterRouter();
        private object lockObj = new Object();
        private readonly Dictionary<Direction, ConcurrentDictionary<int, Client>> recipients;

        private IridiumMasterRouter()
        {
            Array directions = Enum.GetValues(typeof(Direction));
            this.recipients = new Dictionary<Direction, ConcurrentDictionary<int, Client>>(directions.Length);

            // different directions are expected to contain different number of endpoints: 1x of master servers, 10x of game servers, 10000x of clients
            foreach (Direction direction in directions)
            {
                this.recipients[direction] = new ConcurrentDictionary<int, Client>(DefaultConcurrencyLevel, capacity: 64);
            }
        }

        public static IridiumMasterRouter Instance
        {
            get { return instance; }
        }

        public IridiumMasterClientProtocol IridiumMasterClientProtocol { get; set; }

        public bool Send(Packet packet, Destination destination)
        {
            Direction direction = destination.Direction;
            int id = destination.Id;

            Client recipient;

            if (this.TryBeginOperationOnRecipient(direction, id, out recipient, "Send"))
            {
                try
                {
                    switch (direction)
                    {
                        case Direction.Client:
                            recipient.PushSendPacket(packet);
                            break;
                        case Direction.None:
                        case Direction.MasterServer:
                        default:
                            throw new InvalidOperationException(string.Format("Send direction {0} is invalid for this instance of a router", direction));
                    }

                    if (packet == null) // disconnecting
                    {
                        this.UnregisterRecipient(direction, id);
                    }

                    return true;
                }
                finally
                {
                    this.EndOperationOnSession(recipient, "Send");
                }
            }
            else
            {
                return false;
            }
        }

        private bool TryBeginOperationOnRecipient(Direction direction, int id, out Client recipient, string operation = "")
        {
            lock (lockObj)
            {
                recipient = this.GetRecipient(direction, id);
                return recipient != null && this.TryBeginOperationOnSession(recipient, operation);
            }
        }

        private Client GetRecipient(Direction direction, int id)
        {
            Client recipient = null;

            if (!this.recipients[direction].TryGetValue(id, out recipient))
            {
                logger.Debug("{0}:{1} not found in recipients list.", direction, id);
            }

            return recipient;
        }

        private bool TryBeginOperationOnSession(Client client, string operation = "")
        {
            logger.Trace("OperationCount++ {0}", operation);

            if (!client.TryBeginOperation())
            {
                logger.Trace("Operation was denied due to Socket closing.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void EndOperationOnSession(Client client, string operation = "")
        {
            logger.Trace("OperationCount-- {0}", operation);
            client.EndOperation();
        }


        public void UnregisterRecipient(Direction direction, int id)
        {
            // This lock allows userToken to stay alive in case of it's use in Send operation
            lock (logger)
            {
                Client token;
                this.recipients[direction].TryRemove(id, out token);
            }
        }

    }
}
