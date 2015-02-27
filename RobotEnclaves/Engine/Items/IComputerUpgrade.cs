using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Items
{
    using Engine.Computer;

    public interface IComputerUpgrade
    {
        void Apply(IComputer computer);
    }
}
