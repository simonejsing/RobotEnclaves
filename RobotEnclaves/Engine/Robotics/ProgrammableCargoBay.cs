using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Robotics
{
    using Engine.Items;

    public class ProgrammableCargoBay : ProgrammableComponentBase
    {
        private readonly List<CollectableItem> items = new List<CollectableItem>();

        public override string Name
        {
            get
            {
                return "cargobay";
            }
        }

        public IEnumerable<CollectableItem> Items
        {
            get
            {
                return items;
            }
        }

        public override KeyValuePair<string, Func<string[], object>>[] Methods
        {
            get
            {
                return new[]
                       {
                           new KeyValuePair<string, Func<string[], object>>("items", (args) => Items.Select(i => i.Name))
                       };
            }
        }

        public void LoadItem(CollectableItem item)
        {
            items.Add(item);
        }
    }
}
