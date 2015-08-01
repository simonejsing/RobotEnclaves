using System.Collections.Generic;
using Engine.Robotics;

namespace Engine.Network
{
    public interface ICommunicationArray
    {
        float Range { get; }
        IEnumerable<NetworkMessage> Messages { get; }

        void EstablishLink(IRobot targetRobot);
        void BeforeNetworkLink();
        void AfterNetworkLink(NetworkLink link);
        void SendMessage(IRobot targetRobot, NetworkMessagePayload message);
        void ReceiveMessage(NetworkMessage message);
    }
}
