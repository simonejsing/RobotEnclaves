using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Network;

namespace Engine.Robotics
{
    using Engine.Computer;

    public interface IRobot
    {
        IComputer Computer { get; }
        ICommunicationArray Comm { get; }
        IHull Hull { get; }
        IObject Object { get; }

        void Progress(GameTimer timer);
    }
}
