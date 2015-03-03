using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Items
{
    using Engine.Computer;

    public class MemoryUpgrade : CollectableItem, IComputerUpgrade
    {
        private readonly int extraMemory;

        public MemoryUpgrade(int extraMb, string name, string label)
            : base(name, label)
        {
            extraMemory = extraMb;
        }

        public void Apply(IComputer computer)
        {
            computer.MemoryBank.Upgrade(extraMemory);
        }
    }
}
