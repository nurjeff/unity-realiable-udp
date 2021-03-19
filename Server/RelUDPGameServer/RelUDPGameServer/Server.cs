using System;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;

namespace RelUDPGameServer
{
    class ServerObject : INetEventListener
    {
        //Singleton Pattern
        private static ServerObject instance = null;
        private static readonly object padlock = new object();

        ServerObject()
        {
            server = new NetManager(this)
            {
                AutoRecycle = true,
            };
            netPacketProcessor = new NetPacketProcessor();
            InitializePacketProcessor();
            netListener = new EventBasedNetListener();
        }

        public static ServerObject Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ServerObject();
                    }
                    return instance;
                }
            }
        }

        private void InitializePacketProcessor()
        {
            Console.WriteLine("Initializing packet-handlers");
            netPacketProcessor.RegisterNestedType<Packets.TransformData>(() => new Packets.TransformData());
            netPacketProcessor.SubscribeReusable<Packets.TransformPacket>((packet) => PacketHandlers.transformHandler(packet));

        }

        //Networking Code
        public NetManager server;
        public EventBasedNetListener netListener;
        public NetPacketProcessor netPacketProcessor;

        public ClientManager clientManager;

        public void Start()
        {
            clientManager = new ClientManager();
            server.UpdateTime = Constants.MS_PER_TICK;
            server.NatPunchEnabled = true;
            server.Start(12345);
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (server.ConnectedPeersCount < Constants.MAX_PLAYERS)
            {
                Console.WriteLine($"Incoming connection from {request.RemoteEndPoint}");
                request.AcceptIfKey("somekey");

                return;
            }
            Console.WriteLine($"Rejecting incoming connection from {request.RemoteEndPoint}: Server full!");
            request.Reject();
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Console.WriteLine("Network error");
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            netPacketProcessor.ReadAllPackets(reader, server);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            Console.WriteLine("Network unconnected");
        }

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine($"Client {peer.EndPoint} connected.");
            clientManager.AddClient(peer);
            netPacketProcessor.Send(peer, new Packets.WelcomePacket() { ID = 1, WelcomeMessage = "Welcome to the server." }, DeliveryMethod.ReliableOrdered);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            clientManager.RemoveClient(peer);
            switch (disconnectInfo.Reason)
            {
                case (DisconnectReason.Timeout):  Console.WriteLine($"Client {peer.EndPoint} timed out."); break;
                case (DisconnectReason.ConnectionFailed): Console.WriteLine($"Client {peer.EndPoint} failed to connect."); break;
                case (DisconnectReason.HostUnreachable): Console.WriteLine($"Client {peer.EndPoint} was unable to reach host."); break;
                default: Console.WriteLine($"Client {peer.EndPoint} disconnected Unspecified reason."); break;
            }
        }
    }
}