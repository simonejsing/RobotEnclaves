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

        IObject Object { get; }

        IMemoryBank MemoryBank { get; }
        ISensor Sensor { get; set; }

        IEnumerable<IProgrammableComponent> Components { get; }
        IEnumerable<IComputerUpgrade> PendingUpgrades { get; }

        IEnumerable<IProgram> Programs { get; }
        IProgram CurrentProgram { get; }

        void AddProgram(IProgram program);
        void SetCurrentProgram(IProgram program);

        void AddProxyComponents(IEnumerable<IProgrammableComponent> components);

        void InstallUpgrade(IComputerUpgrade upgrade);
        void ApplyUpgrades();

        void ExecuteNextProgramStatement();

        IComputerType EvaluateInstruction(string instruction);
    }
}
