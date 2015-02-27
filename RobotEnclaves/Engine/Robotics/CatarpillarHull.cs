using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    class CatarpillarHull : IHull
    {
        public ProgrammableEngine Engine { get; private set; }
        public ProgrammableCrane Crane { get; private set; }
        public ProgrammableCargoBay CargoBay { get; set; }

        public virtual IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                yield return Engine;
                yield return Crane;
                yield return CargoBay;
            }
        }

        public CatarpillarHull(Robot robot)
        {
            Engine = new ProgrammableEngine();
            Crane = new ProgrammableCrane(robot, 40f);
            CargoBay = new ProgrammableCargoBay(100.0f);
        }
    }
}
