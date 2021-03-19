using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;
using System.Net;
using System.Net.Sockets;
using LiteNetLib.Utils;
using Assets.Scripts.Networking;

public class Client : MonoBehaviour, INetEventListener
{
    //Singleton Pattern
    private static Client _instance;
    public static Client Instance { get { return _instance; } }

    //Required Networking Objects
    private NetManager client;
    private NetPeer server;
    NetPacketProcessor netPacketProcessor;
    public ClientManager clientManager;

    public GameObject playerObject;

    public int clientId;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        Connect("localhost", 12345, "somekey");
        clientManager = new ClientManager();
    }

    public void Connect(string ip, int port, string psk)
    {
        netPacketProcessor = new NetPacketProcessor();

        InitializePacketProcessor();

        client = new NetManager(this)
        {
            AutoRecycle = true,
        };
        client.Start();
        Debug.Log($"Connecting to server {ip}:{port}");
        client.Connect(ip, port, psk);
    }

    //Add handlers
    private void InitializePacketProcessor()
    {
        Debug.Log("Initializing packet-handlers");
        netPacketProcessor.RegisterNestedType<Packets.TransformData>(() => new Packets.TransformData());
        netPacketProcessor.SubscribeReusable<Packets.WelcomePacket>((packet) => PacketHandlers.welcomeHandler(packet));
        netPacketProcessor.SubscribeReusable<Packets.TransformPacket>((packet) => PacketHandlers.transformHandler(packet));

    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
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
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("Established connection.");
        server = peer;
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("Disconnected from server.");
    }

    private void Update()
    {
        if (client != null)
        {
            client.PollEvents();
        }
    }

    private void FixedUpdate()
    {
        if (client != null)
        {
            if (server != null)
            {
                Packets.TransformData data = new Packets.TransformData()
                {
                    x = playerObject.transform.position.x,
                    y = playerObject.transform.position.y,
                    z = playerObject.transform.position.z,
                    rotx = playerObject.transform.rotation.eulerAngles.x,
                    roty = playerObject.transform.rotation.eulerAngles.y,
                    rotz = playerObject.transform.rotation.eulerAngles.z
                };


                netPacketProcessor.Send(server, new Packets.TransformPacket()
                {
                    clientid = this.clientId,
                    transform = data
                }, DeliveryMethod.ReliableOrdered);
            }
        }
    }
}