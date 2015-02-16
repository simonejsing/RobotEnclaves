using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using Engine.Computer;
    using Engine.World;
    using VectorMath;

    public class Robot : IComputer, IObject
    {
        public IMemoryBank MemoryBank { get; private set; }
        public IProgram CurrentProgram { get; set; }

        public Robot()
        {
            MemoryBank = new MemoryBank(200);
        }

        public void ExecuteNextProgramStatement()
        {
            CurrentProgram.GetNextStatement().Execute(this);
        }

        public Vector2 Position { get; set; }
    }
}
