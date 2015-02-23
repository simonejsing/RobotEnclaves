using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Exceptions;
    using Engine.Items;

    public class ProgrammableCrane : ProgrammableComponentBase
    {
        const string RangePropertyName = "range";

        private readonly Robot robot;

        public ProgrammableCrane(Robot robot, float range)
        {
            this.robot = robot;
            this.Range = range;
        }

        public float Range
        {
            get
            {
                return this[RangePropertyName];
            }
            private set
            {
                this[RangePropertyName] = value;
            }
        }

        public override string Name
        {
            get
            {
                return "crane";
            }
        }

        public override string[] Properties
        {
            get
            {
                return new[] { RangePropertyName };
            }
        }

        public override KeyValuePair<string, Func<string[], object>>[] Methods
        {
            get
            {
                return new[]
                       {
                           new KeyValuePair<string, Func<string[], object>>("pickup", this.PickUpNamedItem),
                       };
            }
        }

        private object PickUpNamedItem(string[] arguments)
        {
            var itemName = arguments[0].Substring(1, arguments[0].Length - 2);

            var item = robot.World.FindItemByName(itemName);
            this.PickUpItem(item);
            return null;
        }

        public float PickUpItem(CollectableItem item)
        {
            if(item.Collected)
                throw new RobotException("Attempt to pick up an item that is already owned by a robot.");

            if (ItemInRange(item))
            {
                item.SetPickedUp(robot);
                robot.CargoBay.LoadItem(item);
            }

            return 0;
        }

        public bool ItemInRange(CollectableItem item)
        {
            return robot.ObjectInRange(item, this.Range);
        }
    }
}
