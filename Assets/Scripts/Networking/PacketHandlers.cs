using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using LiteNetLib.Utils;

namespace Packets
{
    public class FooPacket
    {
        public int NumberValue { get; set; }
        public string StringValue { get; set; }
    }

    public class WelcomePacket
    {
        public int ID { get; set; }
        public string WelcomeMessage { get; set; }
    }

    public class TransformPacket
    {
        public int clientid { get; set; }
        public TransformData transform { get; set; }
    }

    public class TransformData : INetSerializable
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float rotx { get; set; }
        public float roty { get; set; }
        public float rotz { get; set; }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(x);
            writer.Put(y);
            writer.Put(z);
            writer.Put(rotx);
            writer.Put(roty);
            writer.Put(rotz);
        }

        public void Deserialize(NetDataReader reader)
        {
            x = reader.GetFloat();
            y = reader.GetFloat();
            z = reader.GetFloat();
            rotx = reader.GetFloat();
            roty = reader.GetFloat();
            rotz = reader.GetFloat();
        }
    }
}


class PacketHandlers
{
    public static void welcomeHandler(Packets.WelcomePacket packet)
    {
        Debug.Log("Message from Server: " + packet.WelcomeMessage);
        Client.Instance.clientId = packet.ID;
    }

    public static void transformHandler(Packets.TransformPacket packet)
    {
        Debug.Log("Got transform from Server");
    }
}