using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Spaceship
{
    using VectorMath;

    public class Spaceship : IObject
    {
        public Vector2 Position { get; set; }
        public UnitVector2 Direction { get; set; }

        public IObjectHealth ObjectHealth { get; private set; }

        public float Mass {
            get
            {
                return 10000.0f;
            }
        }

        public bool Crashed
        {
            get
            {
                return true;
            }
        }

        public Spaceship()
        {
            ObjectHealth = new ObjectHealth(10000);
            ObjectHealth.Destroy();
            Position = Vector2.Zero;
            Direction = UnitVector2.GetInstance(1, 0);
        }
    }
}
