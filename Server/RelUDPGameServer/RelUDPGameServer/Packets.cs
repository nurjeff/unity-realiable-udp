using LiteNetLib.Utils;
using System;
using UnityEngine;

namespace Packets
{
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
    public static void transformHandler(Packets.TransformPacket packet)
    {
        Console.WriteLine("Got transform from Client: " + packet.transform.x + "/" + 
            packet.transform.y + "/" + packet.transform.z + " - " + packet.transform.rotx + "/" + packet.transform.roty + "/" + packet.transform.rotz);
    }
}