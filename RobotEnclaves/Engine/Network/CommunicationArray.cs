using System;
using System.Collections.Generic;
using Engine.Exceptions;
using Engine.Robotics;
using VectorMath;

namespace Engine.Network
{
    public class CommunicationArray : ICommunicationArray
    {
        private const float CommArrayRange = 50.0f;

        public IRobot Owner { get; private set; }

        private readonly List<NetworkMessage> networkMessages = new List<NetworkMessage>(); 
        private readonly List<NetworkLink> networkLinks = new List<NetworkLink>();
        private readonly NetworkTopology network;

        public CommunicationArray(IRobot owner, NetworkTopology network)
        {
            this.Owner = owner;
            this.network = network;
        }

        public float Range
        {
            get { return CommArrayRange; }
        }

        public IEnumerable<NetworkMessage> Messages
        {
            get
            {
                return networkMessages;
            }
        }

        public void EstablishLink(IRobot targetRobot)
        {
            AssertNumberOfConnections();

            if (Vector2.DistanceBetweenSquared(Owner.Object.Position, targetRobot.Object.Position) > CommArrayRange * CommArrayRange)
                throw new CommException("Target robot is out of reach.");

            var link = network.CreateLink(Owner, targetRobot);
            AddNetworkLink(link);
        }

        public void BeforeNetworkLink()
        {
            AssertNumberOfConnections();
        }

        public void AfterNetworkLink(NetworkLink link)
        {
            AddNetworkLink(link);
        }

        public void SendMessage(IRobot targetRobot, NetworkMessagePayload payload)
        {
            network.SendMessage(Owner, targetRobot, new NetworkMessage(Owner, payload));
        }

        public void ReceiveMessage(NetworkMessage message)
        {
            networkMessages.Add(message);
        }

        private void AddNetworkLink(NetworkLink link)
        {
            networkLinks.Add(link);
        }

        private void AssertNumberOfConnections()
        {
            if (networkLinks.Count > 1)
                throw new CommException(string.Format("Robot '{0}' already have two links up.", Owner.Computer.Name));
        }
    }
}