using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Computer.Programs
{
    using Engine.Robotics;

    public class RepairProgram : IProgram
    {
        private readonly IRobot robot;
        private IObjectHealth objectHealth;

        public RepairProgram(IRobot robot)
        {
            this.robot = robot;
        }

        public string Name
        {
            get
            {
                return "repair";
            }
        }

        public bool Finished
        {
            get
            {
                return objectHealth.FullHealth;
            }
        }

        public void Execute(IComputerType arguments)
        {
            var itemName = new ComputerTypeString(arguments);
            var item = robot.Hull.CargoBay.FindItemByName(itemName.Value);
            objectHealth = item.ObjectHealth;
        }

        public IStatement GetNextStatement()
        {
            return new GenericStatement(c => objectHealth.RepairDamage(1));
        }
    }
}
