using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Network;

namespace Engine.Robotics
{
    public class CommBot : Robot
    {
        public CommBot(string name, NetworkTopology network) : base(name)
        {
            this.Comm = new CommunicationArray(this, network);
        }
    }
}
