using System.Collections.Generic;
using PhysicsEngine.Interfaces;
using VectorMath;

namespace PhysicsEngine.Bounding
{
    public abstract class BoundingObject : IBoundingObject
    {
        public abstract IEnumerable<Vector2> Points { get; protected set; }
        public abstract IEnumerable<PointVector2> Connections { get; }
        public abstract IEnumerable<PointVector2> Sides { get; }
        public abstract IBoundingObject Translate(Vector2 vector);
        public abstract IBoundingObject Scale(Vector2 scale);

        public abstract bool Intersects(Vector2 point);
        public abstract bool Intersects(PointVector2 vector);
        public abstract bool Intersects(IBoundingObject obj);
    }
}
