using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Exceptions;
using Engine.Robotics;

namespace Engine.Network
{
    public class NetworkTopology
    {
        private readonly List<NetworkLink> connections = new List<NetworkLink>();
        public IEnumerable<NetworkLink> Connections {
            get
            {
                return connections;
            }
        }

        public NetworkLink CreateLink(IRobot sourceRobot, IRobot targetRobot)
        {
            // Check if the target robot will accept the link
            targetRobot.Comm.BeforeNetworkLink();

            var link = new NetworkLink(sourceRobot, targetRobot);
            var link2 = new NetworkLink(targetRobot, sourceRobot);
            connections.Add(link);
            connections.Add(link2);
            
            targetRobot.Comm.AfterNetworkLink(link);
            return link;
        }

        public void SendMessage(IRobot sourceRobot, IRobot targetRobot, NetworkMessage message)
        {
            if(!RouteExists(sourceRobot, targetRobot))
                throw new CommException(string.Format("No route exists between '{0}' and '{1}'.", sourceRobot.Computer.Name, targetRobot.Computer.Name));

            targetRobot.Comm.ReceiveMessage(message);
        }

        private bool RouteExists(IRobot sourceRobot, IRobot targetRobot)
        {
            List<IRobot> explored = new List<IRobot>();
            Stack<IRobot> frontier = new Stack<IRobot>();
            frontier.Push(sourceRobot);

            while (frontier.Any())
            {
                // Explore
                var node = frontier.Pop();
                if (node == targetRobot)
                    return true;

                if (explored.Contains(node))
                    continue;

                var links = connections.Where(c => c.SourceRobot == node).Select(c => c.TargetRobot);
                foreach (var link in links)
                {
                    frontier.Push(link);
                }

                explored.Add(node);
            }

            return false;
        }
    }
}
