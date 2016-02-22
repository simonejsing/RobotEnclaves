using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicsEngine.Bounding;
using VectorMath;

namespace UnitTestSuite.PhysicsEngine
{
    public class WorldBox : global::PhysicsEngine.Object
    {
        public WorldBox(Vector2 position, Vector2 size) : base(position, size)
        {
            BoundingObject = BoundingPolygon.Box(new Vector2(0, 0), new Vector2(1, 1));
        }
    }
}
