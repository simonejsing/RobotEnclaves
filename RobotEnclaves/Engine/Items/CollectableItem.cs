using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Items
{
    using Engine.World;
    using VectorMath;

    public class CollectableItem : IObject
    {
        public string Name { get; private set; }

        public CollectableItem(string name)
        {
            Name = name;
        }

        public Vector2 Position { get; set; }
    }
}
