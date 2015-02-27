using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Items;
    using Engine.Robotics;

    public interface IComputer
    {
        string Name { get; }

        IMemoryBank MemoryBank { get; }
        ISensor Sensor { get; set; }

        IEnumerable<IProgrammableComponent> Components { get; }
        IEnumerable<IComputerUpgrade> PendingUpgrades { get; }
            
        IProgram CurrentProgram { get; }

        void SetCurrentProgram(IProgram program);

        void InstallUpgrade(IComputerUpgrade upgrade);
        void ApplyUpgrades();
    }
}
