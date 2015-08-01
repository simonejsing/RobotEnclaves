using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Robotics;

namespace Engine.Network
{
    class NullCommunicationArray : ICommunicationArray
    {
        public float Range
        {
            get { return 0.0f; }
        }

        public IEnumerable<NetworkMessage> Messages
        {
            get { return Enumerable.Empty<NetworkMessage>(); }
        }

        public void EstablishLink(IRobot targetRobot)
        {
            throw new InvalidOperationException("Null comm array cannot establish communication link.");
        }

        public void BeforeNetworkLink()
        {
            throw new InvalidOperationException("Null comm array cannot establish communication link.");
        }

        public void AfterNetworkLink(NetworkLink link)
        {
            throw new InvalidOperationException("Null comm array cannot establish communication link.");
        }

        public void SendMessage(IRobot targetRobot, NetworkMessagePayload payload)
        {
            throw new InvalidOperationException("Null comm array cannot send messages.");
        }

        public void ReceiveMessage(NetworkMessage message)
        {
            throw new InvalidOperationException("Null comm array cannot receive messages.");
        }
    }
}
