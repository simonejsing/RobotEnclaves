using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Exceptions;
    using Engine.Items;

    public class ProgrammableCrane : ProgrammableComponentBase
    {
        private readonly Robot robot;

        public ProgrammableCrane(Robot robot, float range)
        {
            var rangeProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "range",
                () => new ComputerTypeFloat(this.Range));

            this.RegisterProperty(rangeProperty);
            this.RegisterMethod(new ProgrammableMethod("pickup", this.PickUpNamedItem));

            this.robot = robot;
            this.Range = range;
        }

        public float Range { get; private set; }

        public override string Name
        {
            get
            {
                return "crane";
            }
            protected set
            {
            }
        }

        private ComputerType PickUpNamedItem(IComputerType arguments)
        {
            var item = this.robot.World.FindItemByName(arguments.ToString());
            return new ComputerTypeBoolean(this.PickUpItem(item));
        }

        public bool PickUpItem(CollectableItem item)
        {
            if(item.Collected)
                throw new RobotException("Attempt to pick up an item that is already owned by a robot.");

            if (ItemInRange(item))
            {
                item.SetPickedUp(robot);
                robot.Hull.CargoBay.LoadItem(item);

                return true;
            }

            return false;
        }

        public bool ItemInRange(IObject item)
        {
            return robot.ObjectInRange(item, this.Range);
        }
    }
}
