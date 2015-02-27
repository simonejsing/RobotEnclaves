using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer
{
    using Engine.Items;
    using Engine.Robotics;

    public class Computer : IComputer
    {
        private readonly List<IComputerUpgrade> pendingUpgrades = new List<IComputerUpgrade>(); 
        public string Name { get; private set; }

        public IMemoryBank MemoryBank { get; private set; }
        public IProgram CurrentProgram { get; set; }
        public ISensor Sensor { get; set; }

        public virtual IEnumerable<IProgrammableComponent> Components
        {
            get
            {
                return Enumerable.Empty<IProgrammableComponent>();
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

        public Computer(string name)
        {
            Name = name;
            MemoryBank = new MemoryBank(200);
            CurrentProgram = null;
            Sensor = null;
        }
    }
}
