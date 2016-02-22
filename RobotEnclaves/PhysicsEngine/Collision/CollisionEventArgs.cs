using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine.Collision
{
    public class CollisionEventArgs
    {
        public Object Target;

        public CollisionEventArgs(Object target)
        {
            Target = target;
        }
    }
}
