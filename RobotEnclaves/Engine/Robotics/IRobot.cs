using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;

    public interface IRobot
    {
        IComputer Computer { get; }
        IHull Hull { get; }
        IObject Object { get; }
    }
}
