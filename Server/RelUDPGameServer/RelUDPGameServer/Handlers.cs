using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;

namespace RelUDPGameServer
{
    class Handlers
    {
        public static void SendWelcome(NetPeer peer)
        {
            ServerObject.Instance.netPacketProcessor.Send(peer, new Packets.WelcomePacket() { ID = 1, WelcomeMessage = "Welcome to the server." }, DeliveryMethod.ReliableOrdered);
        }
    }
}
