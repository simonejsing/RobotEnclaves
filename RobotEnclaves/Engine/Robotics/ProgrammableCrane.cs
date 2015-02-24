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
                () => new ComputerTypeFloat(this.Range),
                ct => { this.Range = ct.Value; },
                ProgrammablePropertyType.ReadOnly);

            this.RegisterProperty(rangeProperty);

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
        }

        public override KeyValuePair<string, Func<ComputerType, ComputerType>>[] Methods
        {
            get
            {
                return new[]
                       {
                           new KeyValuePair<string, Func<ComputerType, ComputerType>>("pickup", this.PickUpNamedItem),
                       };
            }
        }

        private ComputerType PickUpNamedItem(ComputerType arguments)
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
                robot.CargoBay.LoadItem(item);

                return true;
            }

            return false;
        }

        public bool ItemInRange(CollectableItem item)
        {
            return robot.ObjectInRange(item, this.Range);
        }
    }
}
