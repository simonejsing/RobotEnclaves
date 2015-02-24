using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Items
{
    using Engine.Robotics;
    using VectorMath;

    public class CollectableItem : IObject
    {
        public string Name { get; private set; }
        public string Label { get; private set; }
        public Robot OwningRobot { get; private set; }
        public World World { get; private set; }
        public float Mass { get; set; }

        public CollectableItem(string name, string label)
        {
            OwningRobot = null;
            Mass = 1.0f;
            Name = name;
            Label = label;
        }

        private Vector2 itemPosition = Vector2.Zero;

        public Vector2 Position
        {
            get
            {
                if (Collected)
                {
                    return OwningRobot.Position;
                }

                return itemPosition;
            }
            set
            {
                itemPosition = value;
            }
        }

        public bool Discovered { get; private set; }
        public bool Collected {
            get
            {
                return OwningRobot != null;
            }
        }

        public void SetCurrentWorld(World world)
        {
            World = world;
        }

        public void SetDiscovered()
        {
            Discovered = true;
        }

        public void SetPickedUp(Robot byRobot)
        {
            OwningRobot = byRobot;
        }

        public override string ToString()
        {
            return Name + ":" + Label;
        }
    }
}
