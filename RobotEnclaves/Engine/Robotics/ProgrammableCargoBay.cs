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

    public class ProgrammableCargoBay : ProgrammableComponentBase
    {
        private readonly List<CollectableItem> items = new List<CollectableItem>();

        public ProgrammableCargoBay(float capacity)
        {
            var capacityProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "capacity",
                () => new ComputerTypeFloat(this.Capacity));
            var loadProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "load",
                () => new ComputerTypeFloat(this.TotalMass));

            this.RegisterProperty(capacityProperty);
            this.RegisterProperty(loadProperty);
            this.RegisterMethod(new ProgrammableMethod("items", ct => this.ListItems()));

            Capacity = capacity;
        }

        public override string Name
        {
            get
            {
                return "cargobay";
            }
            protected set
            {
            }
        }

        public float Capacity { get; private set; }

        public IList<CollectableItem> Items
        {
            get
            {
                return items;
            }
        }

        public float TotalMass 
        {
            get
            {
                return items.Sum(i => i.Mass);
            }
        }

        private ComputerType ListItems()
        {
            var itemList = this.Items.Select(i => new ComputerTypeString(i.ToString()));
            return new ComputerTypeList(itemList);
        }

        public void LoadItem(CollectableItem item)
        {
            if (Capacity < TotalMass + item.Mass)
            {
                throw new RobotException(string.Format("Cannot pick up '{0}' insufficient room in cargo bay.", item.Name));
            }

            items.Add(item);
        }

        public CollectableItem FindItemByName(string name)
        {
            return Items.First(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
