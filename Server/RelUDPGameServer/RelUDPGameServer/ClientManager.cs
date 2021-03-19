using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;

namespace RelUDPGameServer
{
    public class ClientManager
    {
        public Dictionary<int, NetPeer> clients;

        public ClientManager()
        {
            clients = new Dictionary<int, NetPeer>();
        }

        public void AddClient(NetPeer peer)
        {
            if (clients.Count >= Constants.MAX_PLAYERS)
            {
                return;
            }
            clients.Add(peer.Id, peer);
            LogClients();
        }

        public void RemoveClient(NetPeer peer)
        {
            if (clients.Count > 0)
            {
                clients.Remove(peer.Id);
            }
            LogClients();
        }

        public void LogClients()
        {
            Console.Title = "Server - Clients: " + clients.Count + "/" + Constants.MAX_PLAYERS;
        }
    }
}
