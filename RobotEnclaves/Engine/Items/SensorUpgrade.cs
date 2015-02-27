using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Items
{
    using Engine.Computer;
    using Engine.Robotics;

    public class SensorUpgrade : CollectableItem, IComputerUpgrade
    {
        private readonly ISensor sensor;

        public SensorUpgrade(ISensor sensor, string name, string label)
            : base(name, label)
        {
            this.sensor = sensor;
        }

        public void Apply(IComputer computer)
        {
            computer.Sensor = sensor;
        }
    }
}
