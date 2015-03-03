using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Computer.Programs;
    using Engine.Items;
    using Engine.Robotics;

    public class Computer : IComputer
    {
        private readonly List<IComputerUpgrade> pendingUpgrades = new List<IComputerUpgrade>(); 
        public string Name { get; private set; }

        public IObject Object { get; private set; }

        private readonly MemoryBank memoryBank;

        public IMemoryBank MemoryBank
        {
            get
            {
                return memoryBank;
            }
        }

        public IProgram CurrentProgram { get; private set; }
        public ISensor Sensor { get; set; }

        public virtual IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                yield return memoryBank;
            }
        }

        public IEnumerable<IComputerUpgrade> PendingUpgrades
        {
            get
            {
                return this.pendingUpgrades;
            }
        }

        public void SetCurrentProgram(IProgram program)
        {
            CurrentProgram = program;
        }

        public void InstallUpgrade(IComputerUpgrade upgrade)
        {
            this.pendingUpgrades.Add(upgrade);
        }

        public void ApplyUpgrades()
        {
            foreach (var upgrade in this.pendingUpgrades)
            {
                upgrade.Apply(this);
            }
        }

        public Computer(IObject obj, string name)
        {
            Object = obj;
            Name = name;
            memoryBank = new MemoryBank(200);
            CurrentProgram = new NullProgram();
            Sensor = new NullSensor();
        }
    }
}
