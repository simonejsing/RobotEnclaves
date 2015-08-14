using System.Collections.Generic;
using System.Linq;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine.Bounding
{
    public class NoBounds : IBoundingObject
    {
        public IEnumerable<Vector2> Points
        {
            get { return Enumerable.Empty<Vector2>(); }
        }

        public IEnumerable<PointVector2> Connections
        {
            get { return Enumerable.Empty<PointVector2>(); }
        }

        public IEnumerable<PointVector2> Sides
        {
            get { return Enumerable.Empty<PointVector2>();  }
        }

        public IBoundingObject Translate(Vector2 vector)
        {
            return new NoBounds();
        }

        public IBoundingObject Scale(Vector2 scale)
        {
            return new NoBounds();
        }

        public bool Intersects(Vector2 point)
        {
            return false;
        }

        public bool Intersects(PointVector2 vector)
        {
            return false;
        }

        public bool Intersects(IBoundingObject obj)
        {
            return false;
        }
    }
}
