using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    using VectorMath;

    public class WorldObject : IObject
    {
        public Vector2 Position { get; set; }
        public UnitVector2 Direction { get; set; }
        public IObjectHealth ObjectHealth { get; private set; }
        public float Mass { get; private set; }

        public WorldObject(float mass)
        {
            this.ObjectHealth = new ObjectHealth(100);
            this.Position = Vector2.Zero;
            this.Direction = UnitVector2.GetInstance(1, 0);
            this.Mass = mass;
        }
    }
}
