using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    public interface IHull
    {
        ProgrammableEngine Engine { get; }
        ProgrammableCrane Crane { get; }
        ProgrammableCargoBay CargoBay { get; }

        IEnumerable<IProgrammableComponent> Components { get; }
    }
}
