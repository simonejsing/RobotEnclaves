﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Computer;
    using Engine.Items;

    public class ProgrammableCargoBay : ProgrammableComponentBase
    {
        private readonly List<CollectableItem> items = new List<CollectableItem>();

        public ProgrammableCargoBay(float capacity)
        {
            var capacityProperty = new ProgrammableProperty<ComputerTypeFloat>(
                "capacity",
                () => new ComputerTypeFloat(this.Capacity),
                ct => this.Capacity = ct.Value);

            this.RegisterProperty(capacityProperty);

            Capacity = capacity;
        }

        public override string Name
        {
            get
            {
                return "cargobay";
            }
        }

        public float Capacity { get; private set; }

        public IEnumerable<CollectableItem> Items
        {
            get
            {
                return items;
            }
        }

        public override KeyValuePair<string, Func<ComputerType, ComputerType>>[] Methods
        {
            get
            {
                return new[]
                       {
                           new KeyValuePair<string, Func<ComputerType, ComputerType>>("items", (args) => this.ListItems())
                       };
            }
        }

        private ComputerType ListItems()
        {
            var itemList = this.Items.Select(i => new ComputerTypeString(i.Name)).ToArray();
            return new ComputerTypeList(itemList);
        }

        public void LoadItem(CollectableItem item)
        {
            items.Add(item);
        }
    }
}
