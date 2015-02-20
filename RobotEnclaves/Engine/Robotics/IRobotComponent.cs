using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public interface IRobotComponent
    {
        string Name { get; }
        string[] Properties { get; }

        float this[string propertyName] { get; set; }
    }
}
