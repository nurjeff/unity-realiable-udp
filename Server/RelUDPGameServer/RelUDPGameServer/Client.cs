using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;

namespace RelUDPGameServer
{
    public class Client
    {
        NetPeer peer { get; set; }

        public Client(NetPeer _peer)
        {
            this.peer = _peer;
        }
    }
}
