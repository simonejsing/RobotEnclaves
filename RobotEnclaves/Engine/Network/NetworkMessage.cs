using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Robotics;

namespace Engine.Network
{
    public class NetworkMessage
    {
        public IRobot Sender { get; private set; }
        public NetworkMessagePayload Payload { get; private set; }

        public NetworkMessage(IRobot sender, NetworkMessagePayload payload)
        {
            Sender = sender;
            Payload = payload;
        }
    }
}
