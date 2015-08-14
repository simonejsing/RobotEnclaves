using System.Collections.Generic;
using PhysicsEngine.Collision;

namespace PhysicsEngine.Interfaces
{
    public interface ICollisionObject
    {
        float FrictionCoefficient { get; }
        bool Lethal { get; set; }
        IEnumerable<Collision.Collision> CheckCollision(ObjectTransformation transformation);
        IEnumerable<Violation> CheckViolation(Object obj);
    }
}