using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Computer.Programs;

    public class RepairBot : Robot
    {
        public RepairBot(string name)
            : base(name)
        {
            this.RegisterMethod(new ProgrammableMethod("inspect", this.InspectItem));
            this.Computer.AddProgram(new RepairProgram(this));
        }

        private IComputerType InspectItem(IComputerType ct)
        {
            var itemName = new ComputerTypeString(ct).Value;
            var item = Hull.CargoBay.FindItemByName(itemName);

            return new ComputerTypeString(string.Format("Health: {0}/{1}", item.ObjectHealth.Health, item.ObjectHealth.MaxHealth));
        }
    }
}
