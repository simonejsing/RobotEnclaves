using System.Collections.Generic;
using VectorMath;

namespace PhysicsEngine.Interfaces
{
    public interface IBoundingObject
    {
        IEnumerable<Vector2> Points { get; }
        IEnumerable<PointVector2> Connections { get; }
        IEnumerable<PointVector2> Sides { get; }
        IBoundingObject Translate(Vector2 vector);
        IBoundingObject Scale(Vector2 scale);
        bool Intersects(Vector2 point);
        bool Intersects(PointVector2 vector);
        bool Intersects(IBoundingObject obj);
    }
}
