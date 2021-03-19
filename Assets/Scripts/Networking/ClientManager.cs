using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Networking
{
    public class ClientManager
    {
        public Dictionary<int, OtherClient> clients;

        public ClientManager()
        {
            clients = new Dictionary<int, OtherClient>();
        }

        public void AddClient(int id)
        {
            clients.Add(id, new OtherClient());
        }

        public void RemoveClient(int id)
        {
            if (clients.Count > 0)
            {
                try
                {
                    clients.Remove(id);
                }
                catch (Exception _ex)
                {

                }
                
            }
        }
    }
}
