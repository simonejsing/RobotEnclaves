using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Robotics;

namespace Engine.Network
{
    public class NetworkLink
    {
        public IRobot SourceRobot { get; private set; }
        public IRobot TargetRobot { get; private set; }

        public NetworkLink(IRobot source, IRobot target)
        {
            SourceRobot = source;
            TargetRobot = target;
        }
    }
}
